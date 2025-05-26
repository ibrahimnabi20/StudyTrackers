# Test info

- Name: should add a study entry
- Location: C:\Users\ibrah\Downloads\StudyTrackers\frontend\tests\studytracker.spec.js:3:1

# Error details

```
Error: page.goto: Target page, context or browser has been closed
Call log:
  - navigating to "http://localhost:5179/", waiting until "load"

    at C:\Users\ibrah\Downloads\StudyTrackers\frontend\tests\studytracker.spec.js:4:16
```

# Test source

```ts
   1 | ﻿import { test, expect } from '@playwright/test';
   2 |
   3 | test('should add a study entry', async ({ page }) => {
>  4 |     await page.goto('http://localhost:5179');
     |                ^ Error: page.goto: Target page, context or browser has been closed
   5 |
   6 |     // Vent på at formularen er synlig
   7 |     const subjectInput = page.locator('input[placeholder="Subject"]');
   8 |     const durationInput = page.locator('input[placeholder="Duration (min)"]');
   9 |     const addButton = page.getByRole('button', { name: 'Add Entry' });
  10 |
  11 |     // Vent til felterne findes og er synlige
  12 |     await expect(subjectInput).toBeVisible({ timeout: 5000 });
  13 |     await expect(durationInput).toBeVisible({ timeout: 5000 });
  14 |
  15 |     // Udfyld formularen
  16 |     await subjectInput.fill('Math');
  17 |     await durationInput.fill('45');
  18 |
  19 |     // Klik på knappen
  20 |     await addButton.click();
  21 |
  22 |     // Bekræft at entry vises
  23 |     await expect(page.locator('text=Math')).toBeVisible({ timeout: 5000 });
  24 | });
  25 |
```