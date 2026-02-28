import { defineConfig } from 'vite';
import mkcert from "vite-plugin-mkcert";
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [plugin(), mkcert()],
    server: {
        https: true as any,
        port: 50666,
        strictPort: true,
        open: "/",
        proxy: {
            "/api": {
                target: 'http://localhost:8080',
                changeOrigin: true,
                secure: false
            },
            "/auth": {
                target: 'http://localhost:8080',
                changeOrigin: true,
                secure: false
            },
        }
    },
    
})
