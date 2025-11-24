import { defineConfig, loadEnv, type UserConfigExport } from 'vite';
import plugin from '@vitejs/plugin-react';
import mkcert from "vite-plugin-mkcert";

export default (({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");

  return defineConfig({
    plugins: [mkcert(), plugin()],
    server: {
      https: true,
      port: Number(env.VITE_API_PORT),
      strictPort: true,
      open: "/"
    }
  });
}) satisfies UserConfigExport
