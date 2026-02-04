#!/usr/bin/env node
/**
 * Configurable Jest runner (no package.json script edits).
 * - Reads tools/fe-test.config.json
 * - Supports per-project sequential runs (default) or one-shot all
 * - Emits JSON per run, optional JUnit, and prints concise summaries
 */

const fs = require('fs');
const path = require('path');
const { spawn } = require('child_process');

function resolveRepoRoot(startDir) {
	let dir = startDir;
	while (dir && dir !== path.dirname(dir)) {
		if (fs.existsSync(path.join(dir, 'package.json'))) return dir;
		dir = path.dirname(dir);
	}
	return startDir;
}

const cwd = process.cwd();
const repoRoot = resolveRepoRoot(cwd);
const toolsDir = path.join(repoRoot, 'tools');
const configPath = path.join(toolsDir, 'fe-test.config.json');
const testResultsDir = path.join(repoRoot, 'test-results');
const jsonDir = path.join(testResultsDir, 'json');
const junitDir = path.join(testResultsDir, 'junit');
// Removed file writing - output to console only

function loadConfig() {
	if (!fs.existsSync(configPath)) {
		throw new Error(`Config not found: ${configPath}`);
	}
	const raw = fs.readFileSync(configPath, 'utf8');
	return JSON.parse(raw);
}

function parseArgs(argv) {
	const args = {};
	for (let i = 2; i < argv.length; i++) {
		const a = argv[i];
		if (a === '--project') args.project = argv[++i];
		else if (a === '--maxWorkers') args.maxWorkers = parseInt(argv[++i], 10);
		else if (a === '--maxOldSpaceMB') args.maxOldSpaceMB = parseInt(argv[++i], 10);
		else if (a === '--mode') args.mode = argv[++i]; // sequential | parallelAll
		else if (a === '--detailed') args.detailed = true;
		else if (a === '--enableJUnit') args.enableJUnit = true;
		else if (a === '--noCoverage') args.collectCoverage = false;
		else if (a === '--progress') args.progress = true;
		else if (a === '--detectOpenHandles') args.detectOpenHandles = true;
	}
	return args;
}

function ensureDirs() {
	if (!fs.existsSync(testResultsDir)) fs.mkdirSync(testResultsDir, { recursive: true });
	if (!fs.existsSync(jsonDir)) fs.mkdirSync(jsonDir, { recursive: true });
	if (!fs.existsSync(junitDir)) fs.mkdirSync(junitDir, { recursive: true });
}

function setHeap(maxOldSpaceMB) {
	const existing = process.env.NODE_OPTIONS || '';
	const replaced = /--max-old-space-size=\d+/.test(existing)
		? existing.replace(/--max-old-space-size=\d+/, `--max-old-space-size=${maxOldSpaceMB}`)
		: `${existing} --max-old-space-size=${maxOldSpaceMB}`.trim();
	process.env.NODE_OPTIONS = replaced;
	// Suppress noisy ngcc warnings (e.g., deep imports) to keep CI output clean
	process.env.NGCC_LOG_LEVEL = process.env.NGCC_LOG_LEVEL || 'error';
}

function jestBin() {
	// 1) Node's resolver (handles symlinks, hoisting, and alternate layouts)
	try {
		return require.resolve('jest/bin/jest.js', { paths: [repoRoot] });
	} catch (_) {}
	// 2) Direct paths from repo root
	const local = path.join(repoRoot, 'node_modules', 'jest', 'bin', 'jest.js');
	if (fs.existsSync(local)) return local;
	const localCli = path.join(repoRoot, 'node_modules', 'jest-cli', 'bin', 'jest.js');
	if (fs.existsSync(localCli)) return localCli;
	// 3) From process.cwd() in case runner was invoked from a subdir
	const cwdJest = path.join(cwd, 'node_modules', 'jest', 'bin', 'jest.js');
	if (cwd !== repoRoot && fs.existsSync(cwdJest)) return cwdJest;
	throw new Error(
		'Could not locate Jest binary in node_modules. Run "npm install" from the repo root (e.g. Logitude/AngularModules/AngularModules) and ensure "jest" is in devDependencies.'
	);
}

function runJestOnce(args, env = {}, options = { filterNgccWarnings: false, progress: false, label: '' }) {
	return new Promise((resolve) => {
		const child = spawn(process.execPath, [jestBin(), ...args], {
			cwd: repoRoot,
			env: { ...process.env, ...env }
		});

		 // Simple progress spinner
		const spinnerChars = ['|', '/', '-', '\\'];
		let spinIdx = 0;
		let spinnerTimer = null;
		if (options.progress) {
			spinnerTimer = setInterval(() => {
				process.stdout.write(`\r${spinnerChars[spinIdx]} Running ${options.label || 'tests'}...`);
				spinIdx = (spinIdx + 1) % spinnerChars.length;
			}, 120);
		}

		function writeLine(streamWrite, chunk) {
			const text = chunk.toString();
			if (options.filterNgccWarnings) {
				// Drop noisy deep import warnings from ngcc
				const skip = /Entry point '.*' contains deep imports into .*pdfjs-dist.*pdf_viewer/.test(text) ||
					/Entry point '.*' contains deep imports into .*pdfjs-dist.*build\/pdf/.test(text);
				if (skip) return;
			}
			streamWrite(text);
		}

		child.stdout.on('data', (c) => writeLine((t) => process.stdout.write(t), c));
		child.stderr.on('data', (c) => writeLine((t) => process.stderr.write(t), c));

		child.on('exit', (code) => {
			if (spinnerTimer) {
				clearInterval(spinnerTimer);
				process.stdout.write('\r'); // clear spinner line
			}
			resolve(code || 0);
		});
	});
}

function readJson(p) {
	if (!fs.existsSync(p)) return null;
	try {
		return JSON.parse(fs.readFileSync(p, 'utf8'));
	} catch {
		return null;
	}
}

function formatDuration(seconds) {
	if (seconds < 60) return `${seconds.toFixed(1)}s`;
	const mins = Math.floor(seconds / 60);
	const secs = (seconds % 60).toFixed(1);
	return `${mins}m ${secs}s`;
}

// Removed writeSummaryHeader - no file writing needed

function getCoveragePercentage(project, collectCoverage) {
	if (!collectCoverage) return null;
	
	// Try to read coverage-summary.json from coverage directory
	const coverageSummaryPath = path.join(repoRoot, 'coverage-jest', project, 'coverage-summary.json');
	if (!fs.existsSync(coverageSummaryPath)) {
		// Try root coverage-summary.json (for parallelAll mode)
		const rootCoveragePath = path.join(repoRoot, 'coverage', 'coverage-summary.json');
		if (fs.existsSync(rootCoveragePath)) {
			const coverageData = readJson(rootCoveragePath);
			if (coverageData && coverageData.total) {
				const total = coverageData.total;
				const statements = total.statements?.pct || 0;
				const branches = total.branches?.pct || 0;
				const functions = total.functions?.pct || 0;
				const lines = total.lines?.pct || 0;
				// Calculate average coverage
				const avgCoverage = (statements + branches + functions + lines) / 4;
				return Math.round(avgCoverage * 100) / 100;
			}
		}
		return null;
	}
	
	const coverageData = readJson(coverageSummaryPath);
	if (coverageData && coverageData.total) {
		const total = coverageData.total;
		const statements = total.statements?.pct || 0;
		const branches = total.branches?.pct || 0;
		const functions = total.functions?.pct || 0;
		const lines = total.lines?.pct || 0;
		// Calculate average coverage
		const avgCoverage = (statements + branches + functions + lines) / 4;
		return Math.round(avgCoverage * 100) / 100;
	}
	return null;
}

async function collectCoverageQuickly(maxWorkers, maxOldSpaceMB, projects, mode) {
	console.log('\n📊 Collecting coverage (quick pass)...');
	const coverageArgs = [
		'--coverage',
		'--coverageReporters=json-summary',
		'--maxWorkers',
		String(maxWorkers),
		'--passWithNoTests',
		'--silent'  // Suppress test output, only show coverage
	];
	
	if (mode === 'parallelAll') {
		// Run coverage for all projects at once
		const code = await runJestOnce(coverageArgs, {}, { filterNgccWarnings: true, progress: false, label: 'coverage' });
		return code === 0;
	} else {
		// Run coverage per project
		for (const p of projects) {
			const args = [...coverageArgs, '--selectProjects', p];
			const code = await runJestOnce(args, {}, { filterNgccWarnings: true, progress: false, label: `coverage-${p}` });
			if (code !== 0) return false;
		}
		return true;
	}
}

function displayCoverageGrid(projects, mode) {
	// Get test results for test count and duration
	let totalTests = 0;
	let totalDuration = 0;
	const resultsPath = mode === 'parallelAll'
		? path.join(jsonDir, 'jest-results-all.json')
		: null;
	
	if (resultsPath && fs.existsSync(resultsPath)) {
		const resultsData = readJson(resultsPath);
		if (resultsData) {
			totalTests = resultsData.numTotalTests || 0;
			if (resultsData.testResults && resultsData.testResults.length > 0) {
				let endMs = resultsData.startTime || 0;
				for (const tr of resultsData.testResults) {
					if (tr.perfStats && tr.perfStats.end > endMs) endMs = tr.perfStats.end;
					else if (tr.endTime && tr.endTime > endMs) endMs = tr.endTime;
				}
				if (endMs > resultsData.startTime) {
					totalDuration = Math.round(((endMs - resultsData.startTime) / 1000) * 100) / 100;
				}
			}
		}
	}
	
	const coveragePath = mode === 'parallelAll' 
		? path.join(repoRoot, 'coverage', 'coverage-summary.json')
		: null;
	
	const rows = [];
	let totalStatements = 0, totalBranches = 0, totalFunctions = 0, totalLines = 0;
	let projectCount = 0;
	
	let coverageData = null;
	if (mode === 'parallelAll' && coveragePath && fs.existsSync(coveragePath)) {
		coverageData = readJson(coveragePath);
		if (coverageData && coverageData.total) {
			const t = coverageData.total;
			rows.push({
				project: 'ALL',
				statements: t.statements?.pct || 0,
				branches: t.branches?.pct || 0,
				functions: t.functions?.pct || 0,
				lines: t.lines?.pct || 0
			});
			totalStatements = t.statements?.pct || 0;
			totalBranches = t.branches?.pct || 0;
			totalFunctions = t.functions?.pct || 0;
			totalLines = t.lines?.pct || 0;
			projectCount = 1;
		}
	} else {
		// Per-project coverage
		for (const p of projects) {
			const projectCoveragePath = path.join(repoRoot, 'coverage-jest', p, 'coverage-summary.json');
			if (fs.existsSync(projectCoveragePath)) {
				const data = readJson(projectCoveragePath);
				if (data && data.total) {
					const t = data.total;
					const stmt = t.statements?.pct || 0;
					const brch = t.branches?.pct || 0;
					const func = t.functions?.pct || 0;
					const line = t.lines?.pct || 0;
					rows.push({
						project: p,
						statements: stmt,
						branches: brch,
						functions: func,
						lines: line
					});
					totalStatements += stmt;
					totalBranches += brch;
					totalFunctions += func;
					totalLines += line;
					projectCount++;
				}
			}
		}
	}
	
	if (rows.length === 0) {
		console.log('⚠️  No coverage data found');
		return;
	}
	
	// Calculate averages if multiple projects
	if (projectCount > 1) {
		totalStatements /= projectCount;
		totalBranches /= projectCount;
		totalFunctions /= projectCount;
		totalLines /= projectCount;
	}
	
	// Display grid
	console.log('\n' + '='.repeat(80));
	console.log('📊 COVERAGE SUMMARY');
	console.log('='.repeat(80));
	console.log('');
	
	// Header
	const header = 'Project'.padEnd(25) + 
		'Statements'.padStart(12) + 
		'Branches'.padStart(12) + 
		'Functions'.padStart(12) + 
		'Lines'.padStart(12) + 
		'Average'.padStart(12) +
		'Total'.padStart(12);
	console.log(header);
	console.log('-'.repeat(92));
	
	// Rows
	for (const row of rows) {
		const avg = (row.statements + row.branches + row.functions + row.lines) / 4;
		const total = row.statements + row.branches + row.functions + row.lines;
		const line = row.project.padEnd(25) +
			`${row.statements.toFixed(1)}%`.padStart(12) +
			`${row.branches.toFixed(1)}%`.padStart(12) +
			`${row.functions.toFixed(1)}%`.padStart(12) +
			`${row.lines.toFixed(1)}%`.padStart(12) +
			`${avg.toFixed(1)}%`.padStart(12) +
			`${total.toFixed(1)}%`.padStart(12);
		console.log(line);
	}
	
	// Footer with totals/averages
	if (rows.length > 1) {
		console.log('-'.repeat(92));
		const overallAvg = (totalStatements + totalBranches + totalFunctions + totalLines) / 4;
		const overallTotal = totalStatements + totalBranches + totalFunctions + totalLines;
		const footer = 'AVERAGE'.padEnd(25) +
			`${totalStatements.toFixed(1)}%`.padStart(12) +
			`${totalBranches.toFixed(1)}%`.padStart(12) +
			`${totalFunctions.toFixed(1)}%`.padStart(12) +
			`${totalLines.toFixed(1)}%`.padStart(12) +
			`${overallAvg.toFixed(1)}%`.padStart(12) +
			`${overallTotal.toFixed(1)}%`.padStart(12);
		console.log(footer);
	}
	
	// Add test count and duration at the bottom with per-column calculations
	if (totalTests > 0 || totalDuration > 0) {
		console.log('-'.repeat(92));
		
		// Calculate per-column metrics for Tests row
		let testsStatements = totalTests;
		let testsBranches = totalTests;
		let testsFunctions = totalTests;
		let testsLines = totalTests;
		let testsAverage = totalTests;
		let testsTotal = totalTests;
		
		// If we have coverage data, calculate tests weighted by coverage
		if (coverageData && coverageData.total && totalTests > 0) {
			const t = coverageData.total;
			// Calculate tests per coverage type (weighted by coverage percentage)
			testsStatements = Math.round(totalTests * (t.statements?.pct || 0) / 100);
			testsBranches = Math.round(totalTests * (t.branches?.pct || 0) / 100);
			testsFunctions = Math.round(totalTests * (t.functions?.pct || 0) / 100);
			testsLines = Math.round(totalTests * (t.lines?.pct || 0) / 100);
			testsAverage = Math.round(totalTests * ((t.statements?.pct || 0) + (t.branches?.pct || 0) + (t.functions?.pct || 0) + (t.lines?.pct || 0)) / 400);
		}
		
		// Tests row - show calculated test counts per column
		const testsInfo = 'Tests'.padEnd(25) +
			`${testsStatements}`.padStart(12) +
			`${testsBranches}`.padStart(12) +
			`${testsFunctions}`.padStart(12) +
			`${testsLines}`.padStart(12) +
			`${testsAverage}`.padStart(12) +
			`${testsTotal}`.padStart(12);
		console.log(testsInfo);
		
		// Separator between Tests and Time
		console.log('-'.repeat(92));
		
		// Calculate per-column metrics for Time row
		if (totalDuration > 0) {
			let timeStatements = totalDuration;
			let timeBranches = totalDuration;
			let timeFunctions = totalDuration;
			let timeLines = totalDuration;
			let timeAverage = totalDuration;
			let timeTotal = totalDuration;
			
			// If we have coverage data, calculate time per coverage type (weighted by coverage percentage)
			if (coverageData && coverageData.total) {
				const t = coverageData.total;
				// Calculate time per coverage type (weighted by coverage percentage)
				timeStatements = (totalDuration * (t.statements?.pct || 0) / 100).toFixed(2);
				timeBranches = (totalDuration * (t.branches?.pct || 0) / 100).toFixed(2);
				timeFunctions = (totalDuration * (t.functions?.pct || 0) / 100).toFixed(2);
				timeLines = (totalDuration * (t.lines?.pct || 0) / 100).toFixed(2);
				timeAverage = (totalDuration * ((t.statements?.pct || 0) + (t.branches?.pct || 0) + (t.functions?.pct || 0) + (t.lines?.pct || 0)) / 400).toFixed(2);
			} else {
				timeStatements = totalDuration.toFixed(2);
				timeBranches = totalDuration.toFixed(2);
				timeFunctions = totalDuration.toFixed(2);
				timeLines = totalDuration.toFixed(2);
				timeAverage = totalDuration.toFixed(2);
				timeTotal = totalDuration.toFixed(2);
			}
			
			// Time row - show calculated time per column
			const timeInfo = 'Time'.padEnd(25) +
				`${timeStatements}s`.padStart(12) +
				`${timeBranches}s`.padStart(12) +
				`${timeFunctions}s`.padStart(12) +
				`${timeLines}s`.padStart(12) +
				`${timeAverage}s`.padStart(12) +
				`${timeTotal}s`.padStart(12);
			console.log(timeInfo);
		}
	}
	
	console.log('='.repeat(80));
}

function appendSummaryFromJson(project, jsonPath, detailed, collectCoverage = true) {
	const data = readJson(jsonPath);
	if (!data) return;
	const {
		numTotalTests,
		numPassedTests,
		numFailedTests,
		numPendingTests,
		numTotalTestSuites,
		numPassedTestSuites,
		numFailedTestSuites,
		numPendingTestSuites,
		startTime,
		testResults
	} = data;
	// Calculate duration from test results
	let endMs = startTime;
	for (const tr of testResults || []) {
		// Try perfStats first (Jest format)
		if (tr.perfStats && tr.perfStats.end) {
			if (tr.perfStats.end > endMs) endMs = tr.perfStats.end;
		}
		// Fallback to endTime (some Jest versions)
		else if (tr.endTime && tr.endTime > endMs) {
			endMs = tr.endTime;
		}
	}
	const durationSec =
		endMs && startTime && endMs > startTime 
			? Math.round(((endMs - startTime) / 1000) * 100) / 100 
			: '';

	// Console output only - no file writing
	console.log('\n' + '='.repeat(80));
	console.log(`FE Unit Test Summary (Jest) - ${project.toUpperCase()}`);
	console.log('='.repeat(80));
	console.log('');
	
	// Execution Details
	console.log('📅 EXECUTION DETAILS');
	console.log('-'.repeat(80));
	console.log(`Start Time:     ${new Date(startTime).toLocaleString()}`);
	if (durationSec !== '') {
		const endTime = new Date(startTime + (durationSec * 1000));
		console.log(`End Time:       ${endTime.toLocaleString()}`);
		console.log(`Total Duration: ${durationSec}s (${formatDuration(durationSec)})`);
	}
	console.log('');
	
	// Test Suite Statistics
	console.log('📦 TEST SUITE STATISTICS');
	console.log('-'.repeat(80));
	const suitePassRate = numTotalTestSuites > 0 ? ((numPassedTestSuites / numTotalTestSuites) * 100).toFixed(1) : '0.0';
	console.log(`Total Suites:    ${numTotalTestSuites}`);
	console.log(`  ✅ Passed:     ${numPassedTestSuites} (${suitePassRate}%)`);
	console.log(`  ❌ Failed:     ${numFailedTestSuites}`);
	console.log(`  ⏭️  Skipped:    ${numPendingTestSuites}`);
	console.log('');
	
	// Test Statistics
	console.log('🧪 TEST STATISTICS');
	console.log('-'.repeat(80));
	const testPassRate = numTotalTests > 0 ? ((numPassedTests / numTotalTests) * 100).toFixed(1) : '0.0';
	const testsPerSecond = durationSec > 0 ? (numTotalTests / durationSec).toFixed(2) : '0.00';
	const avgTestTime = numTotalTests > 0 && durationSec > 0 ? (durationSec / numTotalTests * 1000).toFixed(2) : '0.00';
	
	console.log(`Total Tests:     ${numTotalTests}`);
	console.log(`  ✅ Passed:     ${numPassedTests} (${testPassRate}%)`);
	console.log(`  ❌ Failed:     ${numFailedTests}`);
	console.log(`  ⏭️  Skipped:    ${numPendingTests}`);
	console.log('');
	console.log(`Performance:     ${testsPerSecond} tests/sec | Avg: ${avgTestTime}ms per test`);
	console.log('');
	
	// Add coverage percentage if available
	const coveragePct = getCoveragePercentage(project, collectCoverage);
	if (coveragePct !== null) {
		console.log('📊 COVERAGE');
		console.log('-'.repeat(80));
		console.log(`Overall Coverage: ${coveragePct}%`);
		console.log('');
	}

	// Performance Analysis
	const slow = [];
	const fast = [];
	let totalFileTime = 0;
	for (const tr of testResults || []) {
		let fileTime = 0;
		// Try perfStats first (Jest format)
		if (tr.perfStats && tr.perfStats.end && tr.perfStats.start) {
			fileTime = Math.round(((tr.perfStats.end - tr.perfStats.start) / 1000) * 100) / 100;
		}
		// Fallback to endTime/startTime
		else if (tr.endTime && tr.startTime) {
			fileTime = Math.round(((tr.endTime - tr.startTime) / 1000) * 100) / 100;
		}
		
		if (fileTime > 0) {
			totalFileTime += fileTime;
			const fileData = {
				path: tr.name,
				durationSec: fileTime,
				testCount: (tr.assertionResults || []).length
			};
			slow.push(fileData);
			fast.push(fileData);
		}
	}
	
	if (slow.length) {
		console.log('⏱️  PERFORMANCE ANALYSIS');
		console.log('-'.repeat(80));
		slow.sort((a, b) => b.durationSec - a.durationSec);
		fast.sort((a, b) => a.durationSec - b.durationSec);
		
		const avgFileTime = (totalFileTime / slow.length).toFixed(2);
		console.log(`Average file time: ${avgFileTime}s`);
		console.log(`Total file time:   ${totalFileTime.toFixed(2)}s`);
		console.log('');
		
		console.log('🐌 Slowest test files (top 5):');
		for (const s of slow.slice(0, 5)) {
			const testsPerSec = s.testCount > 0 ? (s.testCount / s.durationSec).toFixed(2) : '0.00';
			console.log(`  ${s.path}`);
			console.log(`    Duration: ${s.durationSec}s | Tests: ${s.testCount} | Rate: ${testsPerSec} tests/sec`);
		}
		console.log('');
		
		if (fast.length > 5) {
			console.log('⚡ Fastest test files (top 5):');
			for (const f of fast.slice(0, 5)) {
				const testsPerSec = f.testCount > 0 ? (f.testCount / f.durationSec).toFixed(2) : '0.00';
				console.log(`  ${f.path}`);
				console.log(`    Duration: ${f.durationSec}s | Tests: ${f.testCount} | Rate: ${testsPerSec} tests/sec`);
			}
			console.log('');
		}
	}

	// Failures
	if (numFailedTests > 0) {
		console.log('❌ FAILURES');
		console.log('-'.repeat(80));
		let failureCount = 0;
		for (const tr of testResults || []) {
			for (const ar of tr.assertionResults || []) {
				if (ar.status === 'failed') {
					failureCount++;
					const fullName = ar.fullName || [...(ar.ancestorTitles || []), ar.title].join(' > ');
					const msg = (ar.failureMessages || []).join('\n').replace(/\x1B\[[0-9;]*[mK]/g, '');
					console.log(`[${failureCount}] ${fullName}`);
					console.log(`  File: ${tr.name}`);
					console.log(`  Reason:`);
					console.log(`    ${msg.split(/\r?\n/).join('\n    ')}`.slice(0, 4096));
					console.log('');
				}
			}
		}
		console.log('');
	} else {
		console.log('✅ ALL TESTS PASSED');
		console.log('-'.repeat(80));
		console.log('');
	}

	// Detailed per-file and per-test
	if (detailed) {
		console.log('📋 DETAILED PER-FILE RESULTS');
		console.log('-'.repeat(80));
		for (const tr of testResults || []) {
			let filePassed = 0,
				fileFailed = 0,
				fileSkipped = 0;
			for (const ar of tr.assertionResults || []) {
				if (ar.status === 'passed') filePassed++;
				else if (ar.status === 'failed') fileFailed++;
				else if (ar.status === 'pending') fileSkipped++;
			}
			let fileDur = '';
			// Try perfStats first (Jest format)
			if (tr.perfStats && tr.perfStats.end && tr.perfStats.start) {
				fileDur = Math.round(((tr.perfStats.end - tr.perfStats.start) / 1000) * 100) / 100;
			}
			// Fallback to endTime/startTime
			else if (tr.endTime && tr.startTime) {
				fileDur = Math.round(((tr.endTime - tr.startTime) / 1000) * 100) / 100;
			}
			const totalTests = filePassed + fileFailed + fileSkipped;
			const filePassRate = totalTests > 0 ? ((filePassed / totalTests) * 100).toFixed(1) : '0.0';
			
			console.log('');
			console.log(`📄 ${tr.name}`);
			console.log(`   Duration: ${fileDur !== '' ? `${fileDur}s` : 'N/A'} | Tests: ${totalTests} (✅${filePassed} ❌${fileFailed} ⏭️${fileSkipped}) | Pass Rate: ${filePassRate}%`);

			for (const ar of tr.assertionResults || []) {
				const full = ar.fullName || [...(ar.ancestorTitles || []), ar.title].join(' > ');
				const statusIcon = ar.status === 'passed' ? '✅' : ar.status === 'failed' ? '❌' : '⏭️';
				console.log(`   ${statusIcon} ${full}`);
			}
		}
		console.log('');
	}

	console.log('='.repeat(80));
}

async function main() {
	const cfg = loadConfig();
	const cli = parseArgs(process.argv);

	const maxWorkers = cli.maxWorkers || cfg.maxWorkers || 4;
	const maxOldSpaceMB = cli.maxOldSpaceMB || cfg.maxOldSpaceMB || 8192;
	const mode = cli.mode || cfg.mode || 'sequential';
	const detailed = !!(cli.detailed || cfg.detailed);
	const enableJUnit = !!(cli.enableJUnit || cfg.enableJUnit);
	const progress = !!(cli.progress || cfg.progress);
	const detectOpenHandles = !!(cli.detectOpenHandles || cfg.detectOpenHandles);
	const collectCoverage = cli.collectCoverage === true ? true : (cfg.collectCoverage === true);
	const projects = cfg.projects && Array.isArray(cfg.projects) ? cfg.projects.slice() : [];
	const singleProject = cli.project;

	ensureDirs();
	setHeap(maxOldSpaceMB);

	const baseArgs = [
		'--maxWorkers',
		String(maxWorkers),
		'--passWithNoTests'
		// Removed --verbose for speed (adds overhead)
	];
	if (collectCoverage) baseArgs.push('--coverage');
	// Removed --detectOpenHandles by default (only enable for debugging, adds significant overhead)
	if (detectOpenHandles) baseArgs.push('--detectOpenHandles');

	const env = {};
	const reporters = ['default'];
	if (enableJUnit) {
		// Requires jest-junit to be installed in the project
		reporters.push('jest-junit');
		env.JEST_JUNIT_OUTPUT = path.join(junitDir, 'junit.xml');
		env.JEST_JUNIT_OUTPUT_DIR = junitDir;
		env.JEST_JUNIT_ADD_FILE_ATTRIBUTE = 'true';
	}
	if (progress) {
		// Optional progress bar reporter (requires jest-simple-progress-reporter)
		// If not installed, Jest will error; prefer user to install it in CI/local
		reporters.push('jest-simple-progress-reporter');
	}

	let exitCode = 0;
	if (singleProject) {
		const outJson = path.join(jsonDir, `jest-results-${singleProject}.json`);
		const args = [
			...baseArgs,
			'--json',
			'--outputFile',
			outJson,
			'--selectProjects',
			singleProject,
			'--reporters',
			...reporters
		];
		console.log(`\n=== Running project: ${singleProject} (workers=${maxWorkers}, heap=${maxOldSpaceMB}MB) ===`);
		exitCode = await runJestOnce(args, env, { filterNgccWarnings: true, progress, label: singleProject });
		appendSummaryFromJson(singleProject, outJson, detailed, collectCoverage);
	} else if (mode === 'parallelAll') {
		const outJson = path.join(jsonDir, `jest-results-all.json`);
		const args = ['--json', '--outputFile', outJson, ...baseArgs, '--reporters', ...reporters];
		console.log(`\n=== Running ALL projects in one Jest (workers=${maxWorkers}, heap=${maxOldSpaceMB}MB) ===`);
		exitCode = await runJestOnce(args, env, { filterNgccWarnings: true, progress, label: 'all-projects' });
		appendSummaryFromJson('all', outJson, detailed, collectCoverage);
	} else {
		// sequential
		for (let i = 0; i < projects.length; i++) {
			const p = projects[i];
			console.log(`\n[${i + 1}/${projects.length}] Queueing project: ${p}`);
			const outJson = path.join(jsonDir, `jest-results-${p}.json`);
			const args = [
				...baseArgs,
				'--json',
				'--outputFile',
				outJson,
				'--selectProjects',
				p,
				'--reporters',
				...reporters
			];
			console.log(`\n=== Running project: ${p} (workers=${maxWorkers}, heap=${maxOldSpaceMB}MB) ===`);
			const code = await runJestOnce(args, env, { filterNgccWarnings: true, progress, label: `${p} [${i + 1}/${projects.length}]` });
			appendSummaryFromJson(p, outJson, detailed, collectCoverage);
			if (code !== 0) exitCode = code; // keep last non-zero
		}
	}

	// Summary is already displayed in console via appendSummaryFromJson
	
	// Collect and display coverage after tests (doesn't slow down test execution)
	if (exitCode === 0 && !collectCoverage) {
		try {
			const projectsForCoverage = singleProject ? [singleProject] : projects;
			const modeForCoverage = singleProject ? 'sequential' : mode;
			const coverageSuccess = await collectCoverageQuickly(maxWorkers, maxOldSpaceMB, projectsForCoverage, modeForCoverage);
			if (coverageSuccess) {
				displayCoverageGrid(projectsForCoverage, modeForCoverage);
			}
		} catch (err) {
			console.log(`\n⚠️  Coverage collection failed: ${err.message}`);
		}
	} else if (collectCoverage) {
		// Coverage was collected during tests, just display it
		const projectsForCoverage = singleProject ? [singleProject] : projects;
		const modeForCoverage = singleProject ? 'sequential' : mode;
		displayCoverageGrid(projectsForCoverage, modeForCoverage);
	}
	
	// Write Jenkins-readable status file
	const statusFile = path.join(testResultsDir, 'test-status.txt');
	const status = exitCode === 0 ? 'SUCCESS' : 'FAILURE';
	fs.writeFileSync(statusFile, status, 'utf8');
	
	// Also write JSON status for more detailed Jenkins integration
	const statusJson = {
		status: status,
		exitCode: exitCode,
		timestamp: new Date().toISOString()
	};
	fs.writeFileSync(path.join(testResultsDir, 'test-status.json'), JSON.stringify(statusJson, null, 2), 'utf8');
	
	console.log(`\n📋 Test Status: ${status} (written to ${statusFile})`);
	
	process.exit(exitCode);
}

main().catch((err) => {
	console.error(err);
	process.exit(1);
});


