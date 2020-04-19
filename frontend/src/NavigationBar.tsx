import * as React from "react";

export class NavigationBar extends React.Component {
    render(): React.ReactNode {
        const mainStyle: React.CSSProperties = {
            display: "flex",
            backgroundColor: "rgb(0, 0, 0)",
            height: "40px",
            boxShadow: "0 0 2px black"
        };

        const titleStyle: React.CSSProperties = {
            fontFamily: "'Oswald', sans-serif",
            fontSize: "20pt",
            marginLeft: 10,
            marginTop: 0,
            color: "white"
        };

        const loginStyle: React.CSSProperties = {
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            fontFamily: "'Oswald', sans-serif",
            fontSize: "20pt",
            marginTop: -5,
            marginRight: 40,
            color: "white"
        };

        const registerStyle: React.CSSProperties = {
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            fontFamily: "'Oswald', sans-serif",
            fontSize: "20pt",
            marginTop: -5,
            marginRight: 10,
            color: "white"
        };


        return <div style={mainStyle}>
            <NavigationMenu />
            <div style={titleStyle} >
                RELEASEASY
            </div>
            <div style={{ flexGrow: 1 }}/>
            <div style={loginStyle}>
                sign in
            </div>
            <div style={registerStyle}>
                register
            </div>
        </div>;
    }
}

class NavigationMenu extends React.Component {
    render() {
        const buttons: string[] = [
            "Test1",
            "Test2",
            "Test3"
        ];

        const menuStyle: React.CSSProperties = {
            position: "fixed",
            maxWidth: 0,
            overflow: "hidden"
        };

        return <div>
            <div style={menuStyle}>
                <ul>
                    {buttons.map((item) => <li>{item}</li>)}
                </ul>
            </div>
        </div>;
    }
}