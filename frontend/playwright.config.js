﻿import { defineConfig } from '@playwright/test';

export default defineConfig({
    testDir: './tests',
    timeout: 10000,
    use: {
        baseURL: 'http://localhost:5179',
        headless: true,
    },
});
