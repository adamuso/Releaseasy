import { NavigationBar } from "./NavigationBar";
import * as React from "react";
import { HomePage } from "./pages/HomePage";
import { PageRouting, Route } from "./pages/PageRouting";

interface AppState {
    url: URL,
    page?: any | null;
}

export class App extends React.Component<{}, AppState> {
    constructor(props: {}) {
        super(props);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.state = {
            url: new URL(document.location.href)
        };
    }

    changePage(url: string) {
        history.pushState(null, "Test", url);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.setState({ url: new URL(document.location.href) });
    }

    render() {
        return <div className="app">
            <NavigationBar />
            <div style={{ display: "flex", flexGrow: 1 }}>
                <PageRouting url={this.state.url}>
                    <Route route="/" page={HomePage}/>
                    <Route route="/Dashboard" page={HomePage} />
                </PageRouting>
            </div>
        </div>;
    }
}