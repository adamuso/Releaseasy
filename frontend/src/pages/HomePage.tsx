import { Page } from "./Page";
import * as React from "react";
import { User } from "../backend/User";
import { Application } from "../main";

interface HomePageState {
    username: string,
    password: string
}

export class HomePage extends Page<HomePageState> {
    constructor(props: {}) {
        super(props);

        this.state = {
            username: "",
            password: ""
        };
    }

    render() {
        const style: React.CSSProperties = {
            display: "flex",
            flexDirection: "row",
            fontFamily: "'Oswald', sans-serif",
            color: "white",
            flexGrow: 1
        }

        const boxStyle: React.CSSProperties = {
            display: "flex",
            flexDirection: "column",
            backgroundColor: "rgba(0,0,0,0.85)",
            margin: 60,
            flex: 2,
            borderRadius: 20,
            padding: 10
        }

        const inputStyle: React.CSSProperties = {
            height: 60,
            boxSizing: "border-box",
            borderRadius: 10,
            width: "100%",
            padding: 10,
            fontSize: 28
        }

        const buttonStyle: React.CSSProperties = {
            ...inputStyle,
            padding: 0,
            flexGrow: 1,
            justifyContent: "center",
            alignItems: "center",
            textAlign: "center",
            color: "black",
            fontSize: 40,
            fontWeight: "bold"
        }

        const emptyBoxStyle: React.CSSProperties = {
            display: "flex",
            flex: 1,
            flexDirection: "row",
            margin: 5
        }

        return <div className="home-page" style={style}>
            <div style={{ ...boxStyle, marginRight: 30 }}>
                <div style={{ textAlign: "center", fontSize: 40, marginBottom: 10 }}>ZALOGUJ SIĘ</div>
                <div style={{ marginBottom: 10 }}>
                    <div style={{ fontSize: 28 }}>NAZWA UŻYTKOWNIKA LUB E-MAIL</div>
                    <input value={this.state.username} onChange={(ev) => this.setState({ username: ev.target.value })} style = { inputStyle } type="text" />
                </div>
                <div style={{ marginBottom: 10 }}>
                    <div style={{ fontSize: 28 }}>HASŁO</div>
                    <input value={this.state.password} onChange={(ev) => this.setState({ password: ev.target.value })} style={inputStyle} type="password"/>
                </div>
                <div style={{ display: "flex", marginBottom: 10, flexDirection: "row" }}>
                    <div className="big-button light" onClick={this.loginOnClick.bind(this)} style={{ ...buttonStyle, marginRight: 5 }}>ZALOGUJ</div>
                    <div className="big-button" style={{ ...buttonStyle, marginLeft: 5 }}>PRZYCISK</div>
                </div>
                <div>PRZYPOMNIJ HASŁO</div>
            </div>
            <div style={{...boxStyle, marginLeft: 30 }}>
                <div style={{ textAlign: "center", fontSize: 40, marginBottom: 10 }}>OSTATNIO DODANE PROJEKTY</div>
                <div style={emptyBoxStyle} >
                    <div style={{ ...emptyBoxStyle, backgroundColor: "#484848" }}/>
                    <div style={{ ...emptyBoxStyle, backgroundColor: "#484848" }}/>
                </div>
                <div style={{ ...emptyBoxStyle, backgroundColor: "#484848" }}/>
                <div style={emptyBoxStyle}>
                    <div style={{ ...emptyBoxStyle, backgroundColor: "#484848" }}/>
                    <div style={{ ...emptyBoxStyle, backgroundColor: "#484848" }}/>
                </div>
            </div>
        </div>;
    }

    async loginOnClick() {
        const result = await User.logIn(this.state.username, this.state.password);

        if (result === true) {
            Application.reactApp.changePage("ProjectPage");
        }
    }
}