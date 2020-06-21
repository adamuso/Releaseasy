import { Page } from "./Page";
import React = require("react");
import { Application } from "../main";
import { User } from "../backend/User";
import { App } from "../App";

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
                <button onClick={() => this.onLogin()}>log in</button>
                <div>Forgot password</div>
                <div className="register" onClick={() => Application.reactApp.changePage("Register")}>Register</div>
            </div>
        </div>;
    }

    async onLogin() {
        if (this.state.login.length === 0 || this.state.password.length === 0) {
            return;
        }

        const result = await User.logIn(this.state.login, this.state.password);

        if (result === true) {
            Application.reactApp.changePage("User");
            Application.reactApp.setState({ userLogged: true });
        }
    }
}