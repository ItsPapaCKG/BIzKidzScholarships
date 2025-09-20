import { useState } from "react";
import { Form, useNavigate } from "react-router";

function LoginComponent() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errorState, setErrorState] = useState('');

    const navigate = useNavigate();

    const login = async () => {
        if (!(email.length > 10) || !(password.length > 9)) {
            setErrorState('Username or password is invalid.')
            return;
        }

        await fetch("https://localhost:7095/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ email, password })
        }).then((res) => {
            if (res.status == 200) {
                navigate("/");
            } else {
                setErrorState('Login failed.')
            }
        });
            
    };

    return (
        <div className="border-2 border-danger">
            <form>
                <label>Email:
                    <input name="email" value={email} onChange={e => setEmail(e.target.value)} />
                </label>

                <label>Password:
                    <input name="password" value={password} onChange={e => setPassword(e.target.value)} />
                    </label>

                <button onClick={e => { e.preventDefault(); login(); } }>Submit</button>

                <p className="danger">{errorState}</p>
            </form>
        </div>
  );
}



export default LoginComponent;