import { test, expect } from '@playwright/test';

test('should add a study entry', async ({ page }) => {
    await page.goto('http://localhost:5179');

    const subjectInput = page.locator('input[placeholder="Subject"]');
    const durationInput = page.locator('input[placeholder="Duration (minutes)"]');
    const addButton = page.getByRole('button', { name: 'Add Entry' });

    await expect(subjectInput).toBeVisible({ timeout: 5000 });
    await expect(durationInput).toBeVisible({ timeout: 5000 });

    // Fill out the form
    await subjectInput.fill('Math');
    await durationInput.fill('45');
    await addButton.click();

    // Wait for the new entry to appear
    await expect(page.getByTestId('study-entry').first()).toContainText('Math');
});
