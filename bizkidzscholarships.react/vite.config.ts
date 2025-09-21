import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import mkcert from "vite-plugin-mkcert";
import { config } from './src/services/ConfigService'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin(), mkcert()],
    server: {
        https: true,
        port: 50666,
        strictPort: true,
        open: "/",
    }
})
