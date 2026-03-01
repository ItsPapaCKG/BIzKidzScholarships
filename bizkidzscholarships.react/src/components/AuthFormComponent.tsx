import { useEffect, useState, type ChangeEvent } from "react";
import { Link, useNavigate } from "react-router";
import { type RegisterJSON, UserType } from "../models/ViewModels";
import { ResponseCode } from "../services/APIService";
import { AttemptAuth } from "../services/AuthService";
import "react-datepicker/dist/react-datepicker.css";
import { UseUserAccountContext } from "../contexts/UserAccountContext";

export interface LoginProps {
    RegisterMode?: boolean
}

function AuthFormComponent({ RegisterMode = false }: LoginProps) {
    const userDataContext = UseUserAccountContext();
    const [setIsAuthenticated, _] = [userDataContext.setIsAuthenticated, userDataContext.isAuthenticated];
    const tryGetCookie = userDataContext.populateCookie;

    const [errorState, setErrorState] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const [userForm, setUserForm] = useState<RegisterJSON>({
        FirstName: "",
        LastName: "",
        email: "",
        Birthday: "",
        password: "",
        ConfirmPassword: "",
        UserType: 0
     } as RegisterJSON)
    const [birthday, setDateBirthday] = useState<Date | undefined>(undefined);
    const [validForm, setIsValid] = useState(false);

    const navigate = useNavigate();

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, type } = e.target;

        let parsed: any = value;

        if (name == "UserType") {
            parsed = Number(value);
        }

        if (name == "PrivacyConsent" && e.target.checked) {
            setUserForm(prev => ({ ...prev, IAmOver13: true }));
        }

        if (type == "checkbox") {
            parsed = e.target.checked;
        }

        setUserForm(prev => ({...prev, [name]: parsed}));
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
        if (!formValidate()) {
            return;
        }

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

    const formValidate = () => {
        let valid: boolean = false;
        let emailValid = userForm.email.includes("@");
        let birthdayEntered = userForm.Birthday != null;
        let nameValid = userForm.FirstName != "" && userForm.LastName != "";
        let passwordsMatch = userForm.password == userForm.ConfirmPassword;
        let passwordEntered = userForm.password !== "" && userForm.password.length > 8;

        valid = emailValid && birthdayEntered && nameValid && passwordsMatch && passwordEntered;

        if (!valid) {
            return false;
        }

        const birthDate = userForm.Birthday.split('T')[0];

        const [year, month, day] = birthDate.split("-").map(Number);
        let bday = new Date(year, month - 1, day);
        let today = new Date();

        let thisYear = today.getFullYear();
        let birthYear = bday.getFullYear();

        let age = thisYear - birthYear;

        const hasHadBirthdayThisYear =
            today.getMonth() > bday.getMonth() ||
            (today.getMonth() === bday.getMonth() &&
                today.getDate() >= bday.getDate());

        if (!hasHadBirthdayThisYear) {
            age--;
        }

        let validAge = age >= 13;

        if (!validAge && birthdayEntered) {
            setErrorState("You must be greater than 13 years of age to Register.");
            return false;
        }

        if (!nameValid) {
            setErrorState("You must provide a First and Last name.");
            return false;
        }

        if (!birthdayEntered) {
            setErrorState("You must provide a First and Last name.");
            return false;
        }

        if (!emailValid) {
            setErrorState("You must be greater than 13 years of age to Register.");
            return false;
        }

        if (!passwordEntered) {
            setErrorState("You must enter a password that is 8 characters in length and contain 1 number and 1 special character.");
            return false;
        }

        if (!passwordsMatch) {
            setErrorState("Your passwords do not match.");
            return false;
        }

        return true;
    }

    useEffect(() => {
        if (!RegisterMode) {
            return;
        }

        let valid: boolean = false;
        let emailValid = userForm.email.includes("@");
        let birthdayEntered = userForm.Birthday != null;
        let nameValid = userForm.FirstName != "" && userForm.LastName != "";
        let passwordsMatch = userForm.password == userForm.ConfirmPassword;
        let passwordEntered = userForm.password !== "" && userForm.password.length > 8;

        valid = emailValid && birthdayEntered && nameValid && passwordsMatch && passwordEntered && userForm.MediaConsent && userForm.PrivacyConsent && userForm.IAmOver13;
        
        setIsValid(valid);

    },[userForm]);

    return (
        <div className="popup-wrapper bg-white d-flex justify-content-center align-items-center shadow-lg p-3 rounded-5 h-100 auth-card">
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
                                <p className="text-center">Individuals age 13 or older may create an account with Biz Kidz Scholarships.<br /> <strong>For participants under age 13, a parent or legal guardian must register an account instead.</strong></p>
                                </div>

                            <div className="row justify-content-evenly">
                                <div className="col-auto">
                                    <div className="form-check d-flex align-items-center">
                                        <input type="radio" className="form-check-input me-2" name="UserType" checked={userForm.UserType == UserType.Parent} value={UserType.Parent} onChange={handleChange} />
                                        <label className="form-check-label" aria-for="UserType"><span className="fs-5">Parent</span></label>
                                    </div>
                                </div>
                                <div className="col-auto">
                                    <div className="form-check d-flex align-items-center">
                                        <input type="radio" className="form-check-input me-2" name="UserType" checked={userForm.UserType == UserType.KidOverThirteen} value={UserType.KidOverThirteen} onChange={handleChange} />
                                        <label className="form-check-label" aria-for="UserType"><span className="fs-5">Kid over 13</span></label>
                                    </div>
                                </div>
                            </div>

                                <div className="row mt-4">
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
                                            <input type="email" name="email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.email}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Phone</span>
                                            <input type="tel" name="PhoneNumber" className="form-control" aria-label="Registrant Phone Number" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.PhoneNumber}/>
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
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Confirm Password</span>
                                            <input type="password" name="ConfirmPassword" className="form-control" aria-label="Confirm Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.ConfirmPassword}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-check">
                                        <input type="checkbox" className="form-check-input" name="PrivacyConsent" checked={userForm.PrivacyConsent} onChange={handleChange}></input>

                                        <label className="form-check-label">{ userForm.UserType == UserType.KidOverThirteen && "I confirm that I am at least 13 years old, and " }I agree to the Privacy policy and the Terms and Conditions.<span className="text-danger">*</span></label>
                                        </div>
                                    </div>
                                </div>

                                <div className="row mt-3">
                                    <div className="col">
                                        <div className="form-check">
                                        <input type="checkbox" className="form-check-input" name="MediaConsent" checked={userForm.MediaConsent} onChange={handleChange}></input>

                                            <label className="form-check-label">As a condition of participation, I grant permission for submitted materials to be used for evaluation, program administration, and promotional purposes.<span className="text-danger">*</span></label>
                                        </div>
                                    </div>
                                </div>

                                <div className="row mt-3">
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

                            <div className="row">
                                <div className="col d-flex justify-content-center m-3 text-underline">
                                    <Link to="/passwordreset">Forgot Password</Link>
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