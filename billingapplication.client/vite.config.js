import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { env } from 'process';


const apiUrl = 
  (process.env.BACKEND_HOST && process.env.BACKEND_PORT)
    ? `${process.env.BACKEND_HOST}:${process.env.BACKEND_PORT}`
    : 'http://localhost:5183';


const target = env.ASPNETCORE_URLS
    ? env.ASPNETCORE_URLS.split(';')[0]
    : apiUrl;

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/billingapplication': {
                target,
                secure: false
            }
        },
        host: '0.0.0.0',
        port: 5173,
    }
});