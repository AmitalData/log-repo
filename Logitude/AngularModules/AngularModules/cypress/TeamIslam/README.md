# Test Architecture Refactor

## Current Problems:
1. Mixed test structures (describe vs module-level it)
2. Class files containing test code
3. Module-level it() blocks running automatically
4. Import conflicts causing blank page issues

## New Architecture:
1. **Page Objects**: Pure classes for reusable actions
2. **Test Specs**: Clean describe/it structure
3. **Utilities**: Shared test utilities
4. **Clear Separation**: No test code in class files

## File Structure:
- `page-objects/` - Pure page object classes
- `specs/` - Test specifications
- `utils/` - Test utilities
- `fixtures/` - Test data
