import { z } from "zod";

const envSchema = z.object({
    VITE_API_BASE: z.url(),
    VITE_API_PORT: z.coerce.number()
})

const env = envSchema.parse(import.meta.env);

export const config = {
    baseAPIURL: env.VITE_API_BASE,
    apiPort: env.VITE_API_PORT as Number
}
