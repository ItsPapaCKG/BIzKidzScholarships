export default async function AttemptLogin(email: string, password: string): Promise<number> {
    var res = await fetch("https://localhost:7095/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ email, password })
    });

    return res.status;
}