import { z } from "zod";

const envSchema = z.object({
    VITE_API_BASE: z.url().default("https://localhost/"),
    VITE_API_PORT: z.number().default(80)
})

const env = envSchema.parse(import.meta.env);

export const config = {
    baseAPIURL: env.VITE_API_BASE,
    apiPort: env.VITE_API_PORT
}
