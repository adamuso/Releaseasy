import { Page } from "./Page";
import React = require("react");
import { Application } from "../main";
import { User } from "../backend/User";

export class LogInPage extends Page<{ login: string, password: string }> {
    state = {
        login: "",
        password: "",
    }

    constructor(props: {}) {
        super(props);
    }

    render() {
        return <div className="login-page">
            <div className="login-form">
                <input placeholder="login" onChange={(e) => this.setState({ login: e.target.value })}/>
                <input type="password" placeholder="password" onChange={(e) => this.setState({ password: e.target.value })}/>
                <input type="submit" value="log in" onClick={() => this.onLogin()}/>
                <div>Forgot password</div>
                <div onClick={() => Application.reactApp.changePage("Register")}>Register</div>
            </div>
        </div>;
    }

    async onLogin() {
        if (this.state.login.length === 0 || this.state.password.length === 0) {
            return;
        }

        const result = await User.logIn(this.state.login, this.state.password);

        if (result) {
            Application.reactApp.changePage("User");
        }
    }
}