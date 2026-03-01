
import { useState } from "react";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import { Link, useNavigate } from "react-router-dom";

function PasswordReset() {
    const context = UseUserAccountContext();

    const navigate = useNavigate();

    const [passwordResetConfirmed, setPConfirmed] = [context.passwordResetConfirmed, context.setPasswordResetConfirmed];
    const [requestPasswordReset] = [context.sendPasswordReset];

    const [resetEmail, setResetEmail] = useState<string | undefined>(); 

    return (
        <div className="popup-wrapper bg-white d-flex justify-content-center align-items-center shadow-lg p-3 rounded-5 h-100 auth-card">
            <div className="d-flex flex-column gap-4 p-5">
                <div className="row">
                    <div className="col">
                        <h1>Password Reset</h1>
                    </div>
                </div>
                    {passwordResetConfirmed == undefined && <>
                        <div className="row">
                            <div className="col">
                            <div className="input-group input-group-lg">
                                    <span className="input-group-text" id="inputGroup-sizing-lg">Email</span>
                                    <input type="email" name="email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-lg" onChange={e => setResetEmail(e.target.value)} value={resetEmail} />
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <button className="btn btn-success btn-lg" onClick={() => resetEmail != undefined && requestPasswordReset(resetEmail) }>Request Reset</button>
                        </div>

                    <div className="row">
                        <Link to="/login">Back to Login</Link>
                    </div>
                    </>
                    }

                    {passwordResetConfirmed && <>
                        <div className="row">
                            <p>A password reset link was emailed to the address you have on file.</p>
                            <p className="text-success">The link will expire in 5 minutes.</p>

                            <button className="btn btn-success" onClick={ ()=> navigate("/login") }>Back to Login</button>
                        </div>
                    </>}

                    {passwordResetConfirmed == false && <>
                        <div className="row">
                            <p className="text-danger">There was an error submitting your request.</p>

                            <button className="btn btn-primary" onClick={ () => setPConfirmed(undefined) }>Try Again</button>
                        </div>
                    </>}
                
                
                </div>
        </div>
  );
}

export default PasswordReset;