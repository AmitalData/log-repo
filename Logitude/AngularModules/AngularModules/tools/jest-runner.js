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
const summaryTxt = path.join(testResultsDir, 'summary.txt');
const summaryCsv = path.join(testResultsDir, 'summary.csv');

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
	// Resolve jest binary from local node_modules
	const local = path.join(repoRoot, 'node_modules', 'jest', 'bin', 'jest.js');
	if (fs.existsSync(local)) return local;
	// Fallback: try jest-cli
	const localCli = path.join(repoRoot, 'node_modules', 'jest-cli', 'bin', 'jest.js');
	if (fs.existsSync(localCli)) return localCli;
	throw new Error('Could not locate Jest binary in node_modules.');
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

function writeSummaryHeader() {
	try { fs.unlinkSync(summaryTxt); } catch {}
	try { fs.unlinkSync(summaryCsv); } catch {}
	fs.writeFileSync(summaryTxt, '', 'utf8');
	fs.writeFileSync(
		summaryCsv,
		'project,file,totalSuites,passedSuites,failedSuites,skippedSuites,totalTests,passedTests,failedTests,skippedTests,durationSec\n',
		'utf8'
	);
}

function appendSummaryFromJson(project, jsonPath, detailed) {
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
	let endMs = 0;
	for (const tr of testResults || []) {
		const perf = tr.perfStats;
		if (perf && perf.end > endMs) endMs = perf.end;
	}
	const durationSec =
		endMs && startTime ? Math.round(((endMs - startTime) / 1000) * 100) / 100 : '';

	const lines = [];
	lines.push(`FE Unit Test Summary (Jest) - ${project}`);
	lines.push(`Start Time: ${new Date(startTime).toString()}`);
	if (durationSec !== '') lines.push(`Duration: ${durationSec}s`);
	lines.push(
		`Suites: total=${numTotalTestSuites}, passed=${numPassedTestSuites}, failed=${numFailedTestSuites}, skipped=${numPendingTestSuites}`
	);
	lines.push(
		`Tests:  total=${numTotalTests}, passed=${numPassedTests}, failed=${numFailedTests}, skipped=${numPendingTests}`
	);

	// CSV aggregate row
	const csvAgg = [
		project,
		'ALL',
		numTotalTestSuites,
		numPassedTestSuites,
		numFailedTestSuites,
		numPendingTestSuites,
		numTotalTests,
		numPassedTests,
		numFailedTests,
		numPendingTests,
		durationSec
	].join(',');
	fs.appendFileSync(summaryCsv, `${csvAgg}\n`, 'utf8');

	// Slowest files (top 5)
	const slow = [];
	for (const tr of testResults || []) {
		const perf = tr.perfStats;
		if (perf) {
			slow.push({
				path: tr.name,
				durationSec: Math.round(((perf.end - perf.start) / 1000) * 100) / 100
			});
		}
	}
	if (slow.length) {
		lines.push('');
		lines.push('Slowest test files (top 5):');
		slow.sort((a, b) => b.durationSec - a.durationSec);
		for (const s of slow.slice(0, 5)) lines.push(`- ${s.path} (${s.durationSec}s)`);
	}

	// Failures
	if (numFailedTests > 0) {
		lines.push('');
		lines.push('Failures:');
		for (const tr of testResults || []) {
			for (const ar of tr.assertionResults || []) {
				if (ar.status === 'failed') {
					const fullName = ar.fullName || [...(ar.ancestorTitles || []), ar.title].join(' > ');
					const msg = (ar.failureMessages || []).join('\n').replace(/\x1B\[[0-9;]*[mK]/g, '');
					lines.push(`- ${fullName}`);
					lines.push('  Reason:');
					lines.push(`    ${msg.split(/\r?\n/).join('\n    ')}`.slice(0, 4096));
				}
			}
		}
	}

	// Detailed per-file and per-test
	if (detailed) {
		lines.push('');
		lines.push('Per-file results:');
		for (const tr of testResults || []) {
			let filePassed = 0,
				fileFailed = 0,
				fileSkipped = 0;
			for (const ar of tr.assertionResults || []) {
				if (ar.status === 'passed') filePassed++;
				else if (ar.status === 'failed') fileFailed++;
				else if (ar.status === 'pending') fileSkipped++;
			}
			const perf = tr.perfStats;
			const fileDur =
				perf && perf.end && perf.start
					? Math.round(((perf.end - perf.start) / 1000) * 100) / 100
					: '';
			lines.push(`  ${tr.name}${fileDur !== '' ? ` (${fileDur}s)` : ''}`);
			const csvRow = [
				project,
				`"${tr.name.replace(/"/g, '""')}"`,
				'',
				'',
				'',
				'',
				filePassed + fileFailed + fileSkipped,
				filePassed,
				fileFailed,
				fileSkipped,
				fileDur
			].join(',');
			fs.appendFileSync(summaryCsv, `${csvRow}\n`, 'utf8');

			for (const ar of tr.assertionResults || []) {
				const full = ar.fullName || [...(ar.ancestorTitles || []), ar.title].join(' > ');
				lines.push(`    - [${ar.status}] ${full}`);
			}
		}
	}

	lines.push('');
	fs.appendFileSync(summaryTxt, `${lines.join('\n')}\n`, 'utf8');
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
	const collectCoverage = cli.collectCoverage === false ? false : cfg.collectCoverage !== false;
	const projects = cfg.projects && Array.isArray(cfg.projects) ? cfg.projects.slice() : [];
	const singleProject = cli.project;

	ensureDirs();
	writeSummaryHeader();
	setHeap(maxOldSpaceMB);

	const baseArgs = [
		'--maxWorkers',
		String(maxWorkers),
		'--passWithNoTests',
		'--verbose'
	];
	if (collectCoverage) baseArgs.push('--coverage');
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
		appendSummaryFromJson(singleProject, outJson, detailed);
	} else if (mode === 'parallelAll') {
		const outJson = path.join(jsonDir, `jest-results-all.json`);
		const args = ['--json', '--outputFile', outJson, ...baseArgs, '--reporters', ...reporters];
		console.log(`\n=== Running ALL projects in one Jest (workers=${maxWorkers}, heap=${maxOldSpaceMB}MB) ===`);
		exitCode = await runJestOnce(args, env, { filterNgccWarnings: true, progress, label: 'all-projects' });
		appendSummaryFromJson('all', outJson, detailed);
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
			appendSummaryFromJson(p, outJson, detailed);
			if (code !== 0) exitCode = code; // keep last non-zero
		}
	}

	console.log('\n=== Results ===');
	console.log(`Summary: ${summaryTxt}`);
	console.log(`CSV:     ${summaryCsv}`);
	process.exit(exitCode);
}

main().catch((err) => {
	console.error(err);
	process.exit(1);
});


