import { Page } from "./Page";
import * as React from "react";

export class HomePage extends Page {
    render() {
        const style: React.CSSProperties = {
            display: "flex",
            flexDirection: "column",
            fontFamily: "'Oswald', sans-serif",
            color: "white"
        }

        return <div style={style}>
            <div>
                <div>ZALOGUJ SIĘ</div>
                <div>
                    <div>NAZWA UŻYTKOWNIKA LUB E-MAIL</div>
                    <input type="text"/>
                </div>
                <div>
                    <div>HASŁO</div>
                    <input type="password"/>
                </div>
                <div>
                    <div>ZALOGUJ</div>
                    <div>PRZYCISK</div>
                </div>
                <div>PRZYPOMNIJ HASŁO</div>
            </div>
            <div>

            </div>
        </div>;
    }
}