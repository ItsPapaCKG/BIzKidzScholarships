import { useEffect, useState, type ChangeEvent } from "react";
import { Form, Link, useNavigate } from "react-router";
import { type LoginJSON, type IUserProfile, type RegisterJSON } from "../models/ViewModels";
import { APICall, ResponseCode, ResponseError } from "../services/APIService";
import { AttemptAuth } from "../services/AuthService";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { UseUserAccountContext } from "../contexts/UserAccountContext";

export interface LoginProps {
    RegisterMode?: boolean
}

function AuthFormComponent({ RegisterMode = false }: LoginProps) {
    const userDataContext = UseUserAccountContext();
    const [setIsAuthenticated, isAuthenticated] = [userDataContext.setIsAuthenticated, userDataContext.isAuthenticated];
    const tryGetCookie = userDataContext.populateCookie;

    const [errorState, setErrorState] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const [userForm, setUserForm] = useState<RegisterJSON>({
        FirstName: "",
        LastName: "",
        email: "",
        Birthday: "",
        password: "",
        ConfirmPassword: ""
     } as RegisterJSON)
    const [birthday, setDateBirthday] = useState<Date | undefined>(undefined);
    const [validForm, setIsValid] = useState(false);

    const navigate = useNavigate();

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setUserForm(prev => ({...prev, [name]: value}));
    }

    const login = async () => {
        let res = await AttemptAuth(userForm);
        if (res.success) {
            await tryGetCookie();

            setIsAuthenticated(true);

            navigate("/");
            return;
        }

        let message = "Login failed. ";

        // TODO SWITCH
        if (res.error.status == ResponseCode.Unauthorized) {
            message += "Incorrect username or password."
        } else {
            message += "Unspecified error occurred (" + ResponseCode[res.error.status] + ")";
        }

        setErrorState(message);
    };

    const register = async () => {
        let res = await AttemptAuth(userForm, true);

        if (!res.success)
        {
            setErrorState(res.error.message ?? "An unspecified error has occurred. Please contact us for assistance.");
            return;
        }

        setIsLoading(true);

        // buffer to allow the server to internally process the registration
        //await setTimeout(() => {}, 5000);

        await login();
        setIsLoading(false);
    };

    const setBirthday = (date: Date | undefined) => {
        let d = date?.toISOString() ?? "";
        setDateBirthday(date);

        setUserForm(prev => ({...prev, ["Birthday"]: d ?? ""}));
    };

    useEffect(() => {
        console.log("IsAuthenticated when Auth component loads: " + isAuthenticated);
        let valid: boolean = userForm.email.includes("@");
        valid = userForm.Birthday != null;
        valid = userForm.FirstName != "" && userForm.LastName != "";
        valid = userForm.password == userForm.ConfirmPassword;
        valid = userForm.password.length > 0;
        
        if (valid) {
            setIsValid(true);
            return;
        } else {
            setIsValid(false);
        }

    },[userForm]);

    return (
        <div className="popup-wrapper bg-white d-flex justify-content-center align-items-center shadow-lg rounded-5 h-100 auth-card">
                <form>
                    <div className="container">
                        {RegisterMode ? (
                            <>
                                <div className="row">
                                    <div className="col d-flex justify-content-center mt-3 mb-0 text-center">
                                        <h1>Register your Account</h1>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col d-flex justify-content-center m-3 mb-5 text-underline">
                                        <Link to="/login">Already have an account?</Link>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col-12 col-md-6">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-lg">First Name</span>
                                            <input type="text" name="FirstName" className="form-control" aria-label="Registrant First Name" aria-describedby="inputGroup-sizing-lg" onChange={handleChange}  value={userForm.FirstName}/>
                                        </div>
                                    </div>

                                    <div className="col-12 col-md-6">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Last Name</span>
                                            <input type="text" name="LastName" className="form-control" aria-label="Registrant Last Name" aria-describedby="inputGroup-sizing-sefault" onChange={handleChange}  value={userForm.LastName}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col m-0">

                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Birthday</span>
                                            
                                            {/*<div className="flex-grow-1">*/}
                                            {/*    <DatePicker*/}
                                            {/*        className="form-control rounded-0 rounded-end w-100"*/}
                                            {/*        onChange={(date) => {setBirthday(date);}}*/}
                                            {/*        selected={birthday}*/}
                                            {/*        dateFormat="yyyy-MM-dd"*/}
                                            {/*        isClearable*/}
                                            {/*        placeholderText="Select your birthday..."*/}
                                            {/*        aria-describedby="inputGroup-sizing-default"*/}
                                            {/*        showYearDropdown*/}
                                            {/*    />*/}
                                        {/*</div>*/}

                                            <input
                                                className="form-control"
                                                type="date"
                                                value={birthday ? birthday.toISOString().slice(0, 10) : ""}
                                                onChange={(e) => setBirthday(e.target.valueAsDate ?? undefined)}
                                            />
                                        </div>

                                        

                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Email</span>
                                            <input type="email" name="Email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.email}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Phone Number</span>
                                            <input type="tel" name="PhoneNumber" className="form-control" aria-label="Registrant Phone Number" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.PhoneNumber}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                                            <input type="password" name="Password" className="form-control" aria-label="Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.password}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Confirm Password</span>
                                            <input type="password" name="ConfirmPassword" className="form-control" aria-label="Confirm Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.ConfirmPassword}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <button onClick={e => { e.preventDefault(); register(); } } disabled={!validForm || isLoading} className="btn btn-lg btn-success w-100">{ isLoading ? "Loading..." : "Register"}</button>
                                    </div>
                                </div>
                            </>
                        ) : 
                        (<>
                            <div className="row">
                                <div className="col d-flex justify-content-center mt-3 mb-0">
                                    <h1>Existing User</h1>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="input-group input-group-lg mb-3 mt-3">
                                        <span className="input-group-text" id="inputGroup-sizing-default">Email</span>
                                        <input type="email" name="email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.email}/>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="input-group input-group-lg mb-3">
                                        <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                                        <input type="password" name="password" className="form-control" aria-label="Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.password}/>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <button onClick={e => { e.preventDefault(); login(); } } className="btn btn-lg btn-success">Login</button>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col d-flex justify-content-center m-3 text-underline">
                                    <Link to="/register">Create a New Account</Link>
                                </div>
                            </div>
                            
                        </>)
                        }
                        

                        <p className="text-danger" style={{whiteSpace: "pre-line"}}>{errorState}</p>
                    </div>
                    
                </form>
        </div>
  );
}



export default AuthFormComponent;