#!/usr/bin/env node
/**
 * CI install: remove node_modules then npm install.
 * Fixes "Invalid package name __ngcc_entry_points__.json" when npm reuses a broken node_modules.
 * Use: node tools/ci-install.js   or   npm run ci-install
 */

const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

function resolveRepoRoot(startDir) {
	let dir = startDir;
	while (dir && dir !== path.dirname(dir)) {
		if (fs.existsSync(path.join(dir, 'package.json'))) return dir;
		dir = path.dirname(dir);
	}
	return startDir;
}

const repoRoot = resolveRepoRoot(process.cwd());
const nodeModules = path.join(repoRoot, 'node_modules');

if (fs.existsSync(nodeModules)) {
	console.log('Removing node_modules...');
	const isWin = process.platform === 'win32';
	execSync(isWin ? 'rmdir /s /q node_modules' : 'rm -rf node_modules', {
		cwd: repoRoot,
		stdio: 'inherit'
	});
}

console.log('Running npm install...');
execSync('npm install', { cwd: repoRoot, stdio: 'inherit' });
