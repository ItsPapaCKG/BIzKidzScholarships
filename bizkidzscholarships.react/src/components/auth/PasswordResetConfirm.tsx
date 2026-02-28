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

    const [passwordResetRequest, setPasswordResetRequest] = useState({
        Password: "",
        Token: token,
        Email: email,
        ConfirmPassword: ""
    } as PasswordResetModel);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, type } = e.target;

        setPasswordResetRequest(prev => ({ ...prev, [name]: value }));
    };

    const resetPassword = async () => {

        let res = await APICall("ResetPassword", "POST", passwordResetRequest, true);

        if (res.success) {
            setPasswordResetConfirmed(true);
            return;
        }

        setPasswordResetConfirmed(false);
    }

    return (
      <div className="container d-flex justify-content-center align-items-center">
          <div className="card">
              <div className="card-header">
                  <h1>Password Reset</h1>
              </div>
              {passwordResetConfirmed == undefined && <>
                  <div className="card-body">
                      <div className="form-check d-flex align-items-center">
                          <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                          <input type="password" name="Password" className="form-control" aria-label="Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={passwordResetRequest.Password} />
                      </div>

                      <div className="form-check d-flex align-items-center">
                          <span className="input-group-text" id="inputGroup-sizing-default">Confirm</span>
                          <input type="password" name="ConfirmPassword" className="form-control" aria-label="ConfirmPassword" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={passwordResetRequest.ConfirmPassword} />
                      </div>
                  </div>

                  <div className="card-footer">
                      <button className="btn btn-success btn-lg" onClick={e => passwordResetRequest.Password == passwordResetRequest.ConfirmPassword && resetPassword() }>Request Reset</button>
                    </div>


              </>
              }

              {passwordResetConfirmed && <>
                  <div className="card-body">
                      <p>The password reset was successful.</p>

                      <button className="btn btn-success" onClick={e => navigate("/login")}>Back to Login</button>
                  </div>
              </>}

              {passwordResetConfirmed == false && <>
                  <div className="card-body">
                      <p className="text-danger">There was an error submitting your request.</p>

                      <button className="btn btn-primary" onClick={e => setPasswordResetConfirmed(undefined)}>Try Again</button>
                  </div>
              </>}

          </div>
      </div>
  );
}

export default PasswordResetConfirm;