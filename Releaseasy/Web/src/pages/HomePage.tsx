import { Page } from "./Page";
import * as React from "react";

export class HomePage extends Page {
    render() {
        const style = {
            backgroundColor: "red",
            height: "10px"
        }

        return <div style={style} />;
    }
}