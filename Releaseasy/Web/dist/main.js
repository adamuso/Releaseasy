import * as ReactDOM from "react-dom";
import * as React from "react";
import { App } from "./App";
const appRef = React.createRef();
ReactDOM.render(React.createElement(App, { ref: appRef }), document.body);
export class Application {
    get reactApp() {
        return appRef.current;
    }
}
//# sourceMappingURL=main.js.map