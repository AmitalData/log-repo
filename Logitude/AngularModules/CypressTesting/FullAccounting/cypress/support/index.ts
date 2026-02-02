// ***********************************************************
// This example support/index.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
//import './commands'

import '../../../Base/cypress/commands/Authentication';
import '../../../Base/cypress/commands/Elements';
import '../../../Base/cypress/commands/AliasDefinitions';

import addContext from 'mochawesome/addContext';
// Add screenshot to report on test failure
Cypress.on('test:after:run', (test, runnable) => {
  if (test.state === 'failed') {
    let item = runnable
    const nameParts = [runnable.title]

    // Iterate through all parents and grab the titles
    while (item.parent) {
      nameParts.unshift(item.parent.title)
      item = item.parent
    }

    const fullTestName = nameParts
      .filter(Boolean)
      .join(' -- ') // how Cypress joins test title fragments

    const imageUrl = `screenshots/${
      Cypress.spec.name
    }/${fullTestName} (failed).png`

    addContext({ test }, imageUrl)
  }
})

// Ignore known uncaught cross-origin / non-critical errors
Cypress.on('uncaught:exception', (err) => {
  if (
    err.message.includes('Script error') ||
    err.message.includes('cross-origin') ||
    err.message.includes('ResizeObserver')
  ) {
    return false
  }
})


// Alternatively you can use CommonJS syntax:
// require('./commands')
