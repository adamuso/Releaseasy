import * as React from "react";

export class NavigationBar extends React.Component {
    render(): React.ReactNode {
        const mainStyle: React.CSSProperties = {
            display: "flex",
            backgroundColor: "rgba(0, 0, 0, 0.3)",
            height: "80px"
        };

        const titleStyle: React.CSSProperties = {
            fontFamily: "'Oswald', sans-serif",
            fontSize: "60pt",
            marginTop: -20,
            color: "white"
        };

        const helpStyle: React.CSSProperties = {
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            fontFamily: "'Oswald', sans-serif",
            fontSize: "50pt",
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
            <div style={helpStyle}>
                ?
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
            <div>Logo</div>
            <div style={menuStyle}>
                <ul>
                    {buttons.map((item) => <li>{item}</li>)}
                </ul>
            </div>
        </div>;
    }
}