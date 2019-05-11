import * as ReactDOM from "react-dom"
import * as React from "react";
import { App } from "./App";

const appRef = React.createRef<App>();

ReactDOM.render(
    <App ref={appRef} />,
    document.getElementById("app")
);

export class Application
{
    get reactApp() { 
        return appRef.current!;
    }
}