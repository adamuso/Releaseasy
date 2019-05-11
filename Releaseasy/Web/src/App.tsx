import { NavigationBar } from "./NavigationBar";
import * as React from "react";
import { HomePage } from "./pages/HomePage";
import { PageRouting, Route } from "./pages/PageRouting";

interface AppState {
    page: any | null;
}

export class App extends React.Component<{}, AppState> {
    url: URL;

    constructor(props: {}) {
        super(props);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.url = new URL(document.location.href);
    }

    render() {
        return <div>
            <NavigationBar />
            <div>
                <PageRouting url={this.url}>
                    <Route route="/" page={HomePage}/>
                </PageRouting>
            </div>
        </div>;
    }
}