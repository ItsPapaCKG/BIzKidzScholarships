import { useState, type ChangeEvent } from "react";

import { useNavigate, useSearchParams } from "react-router-dom";
import type { PasswordResetModel } from "../../models/ViewModels";
import { APICall } from "../../services/APIService";

function PasswordResetConfirm() {
    const [search] = useSearchParams();
    const navigate = useNavigate();

    var email = search.get("email");
    var token = search.get("token");

    const [passwordResetConfirmed, setPasswordResetConfirmed] = useState<boolean | undefined>();
    const [error, setError] = useState<string | undefined >();

    const [passwordResetRequest, setPasswordResetRequest] = useState({
        Password: "",
        Token: token,
        Email: email,
        ConfirmPassword: ""
    } as PasswordResetModel);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setPasswordResetRequest(prev => ({ ...prev, [name]: value }));
    };

    const resetPassword = async () => {
        setError(undefined);
        let res = await APICall("ResetPassword", "POST", passwordResetRequest, true);

        if (res.success) {
            setPasswordResetConfirmed(true);
            return;
        }

        setPasswordResetConfirmed(false);
        setError(res.error.message);
    }

    return (
        <div className="popup-wrapper bg-white d-flex shadow-lg p-3 rounded-5 h-100 auth-card">
          <div className="d-flex flex-column container p-4 gap-3">
              
                <div className="row">
                    <div className="col">
                        <h1>Password Reset</h1>
                    </div>
                </div>

                  {passwordResetConfirmed == undefined && <>
                    <div className="row">
                        <div className="col">
                            <div className="input-group">
                                <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                                <input type="password" name="Password" className="form-control" aria-label="Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={passwordResetRequest.Password} />
                            </div>
                        </div>
                    </div>
                    <div className="row p-0">
                           <div className="col ">
                                <div className="input-group">
                                    <span className="input-group-text" id="inputGroup-sizing-default">Confirm</span>
                                    <input type="password" name="ConfirmPassword" className="form-control" aria-label="ConfirmPassword" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={passwordResetRequest.ConfirmPassword} />
                                </div>
                            </div>
                    </div>

                    <div className="row">
                        <div className="col ">
                            <button className="btn btn-success btn-lg" onClick={() => resetPassword()} disabled={ passwordResetRequest.Password != passwordResetRequest.ConfirmPassword }>Request Reset</button>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col ">
                            <p className="text-danger">{ error != undefined && error }</p>
                        </div>
                    </div>


                  </>
                  }

                  {passwordResetConfirmed && <>
                    <div className="row">
                        <div className="col">
                          <p>The password reset was successful.</p>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <button className="btn btn-success" onClick={() => navigate("/login")}>Back to Login</button>
                        </div>
                    </div>
                  </>}

                  {passwordResetConfirmed == false && <>
                      <div className="card-body">
                          <p className="text-danger">There was an error submitting your request.</p>

                          <button className="btn btn-primary" onClick={() => setPasswordResetConfirmed(undefined)}>Try Again</button>
                      </div>
                  </>}

                </div>
      </div>
  );
}

export default PasswordResetConfirm;