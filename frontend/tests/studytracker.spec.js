import { test, expect } from '@playwright/test';

test('should add a study entry', async ({ page }) => {
    await page.goto('http://localhost:5179');

    // Vent på at formularen er synlig
    const subjectInput = page.locator('input[placeholder="Subject"]');
    const durationInput = page.locator('input[placeholder="Duration (min)"]');
    const addButton = page.getByRole('button', { name: 'Add Entry' });

    // Vent til felterne findes og er synlige
    await expect(subjectInput).toBeVisible({ timeout: 5000 });
    await expect(durationInput).toBeVisible({ timeout: 5000 });

    // Udfyld formularen
    await subjectInput.fill('Math');
    await durationInput.fill('45');

    // Klik på knappen
    await addButton.click();

    // Bekræft at entry vises
    await expect(page.locator('text=Math')).toBeVisible({ timeout: 5000 });
});
