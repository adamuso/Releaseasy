import { Page } from "./Page";
import React = require("react");
import { TextInput } from "../components/TextInput";
import { ValidationContext } from "../utility/ValidationContext";
import { User } from "../backend/User";
import { Application } from "../main";
import { validateEmail } from "../utility/Validators";

type RegistrationType = "employee" | "company";

interface RegisterPageState {
    type: RegistrationType;
    name: string;
    lastName: string;
    location: string;
    email: string;
    password: string;
    confirmPassword: string;
    error: string;
}

export class RegisterPage extends Page<RegisterPageState> {
    state = {
        type: "employee" as RegistrationType,
        name: "",
        lastName: "",
        location: "",
        email: "",
        password: "",
        confirmPassword: "",
        error: ""
    };

    private validationContext = new ValidationContext();

    constructor(props: {}) {
        super(props);
    }

    render() {
        const $ = this.binder;
        const emailValidator = (message: string) => {
            return (email: string) => {
                if (!validateEmail(email)) {
                    return message;
                }

                return true;
            };
        };

        const nonEmptyValidator = (message: string) => {
            return (text: string) => {
                if (!text || text.length === 0) {
                    return message;
                }

                return true;
            };
        };

        const passwordValidator = (message: string) => {
            return (password: string) => {
                if (password.length < 8) {
                    return message;
                }

                return true;
            };
        };

        const confirmPasswordValidator = (message: string) => {
            return (password: string) => {
                if (password !== this.state.password) {
                    return message;
                }

                return true;
            };
        };

        return <div className="register-page">
            <div className="register-form">
                <div className="type">
                    <div onClick={() => this.selectType("employee")} className={this.state.type === "employee" ? "selected" : ""}>Employee</div>
                    <div onClick={() => this.selectType("company")} className={this.state.type === "company" ? "selected" : ""}>Company</div>
                </div>
                {this.state.type === "employee" ? <div className="data">
                    <TextInput placeholder="name" value={$("name")} validator={nonEmptyValidator("Name cannot be empty")} validationContext={this.validationContext}/>
                    <TextInput placeholder="last name" value={$("lastName")} validator={nonEmptyValidator("Last name cannot be empty")} validationContext={this.validationContext}/>
                    <TextInput placeholder="email" value={$("email")} validator={emailValidator("Specified email is invalid")} validationContext={this.validationContext}/>
                    <TextInput password={true} placeholder="password" value={$("password")} validator={passwordValidator("Password need to be at least 8 characters long")} validationContext={this.validationContext}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")} validator={confirmPasswordValidator("Passwords are not the same")} validationContext={this.validationContext}/>
                </div> : <div className="data">
                    <TextInput placeholder="company name" value={$("name")} validator={nonEmptyValidator("Name cannot be empty")} validationContext={this.validationContext}/>
                    <TextInput placeholder="company address" value={$("location")} validator={nonEmptyValidator("Address cannot be empty")} validationContext={this.validationContext}/>
                    <TextInput placeholder="email" value={$("email")} validator={emailValidator("Specified email is invalid")} validationContext={this.validationContext}/>
                    <TextInput password={true} placeholder="password" value={$("password")} validator={passwordValidator("Password need to be at least 8 characters long")} validationContext={this.validationContext}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")} validator={confirmPasswordValidator("Passwords are not the same")} validationContext={this.validationContext}/>
                </div>}
                <div className="terms">
                    <input type="checkbox"/>
                    <div>I agree to the Releaseasy Terms and Privacy.</div>
                </div>
                <div className={"error" + (this.state.error ? " visible" : "")}>{this.state.error}</div>
                <button onClick={() => this.onRegister()}>register</button>
            </div>
        </div>;
    }

    selectType(type: RegistrationType) {
        this.setState({
            type
        });
    }

    async onRegister() {
        if (!this.validationContext.validate()) {
            return;
        }

        const result = await User.register(
            this.state.type,
            this.state.name,
            this.state.lastName,
            this.state.location,
            this.state.email,
            this.state.password,
            this.state.email
        );

        if (result === true) {
            this.setState({ error: "" });
            Application.reactApp.changePage("Login");
        }
        else {
            this.setState({ error: "There was an error while registering a new user or company." });
        }
    }
}