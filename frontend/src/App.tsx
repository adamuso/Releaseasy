import { NavigationBar } from "./NavigationBar";
import * as React from "react";
import { HomePage } from "./pages/HomePage";
import { PageRouting, Route } from "./pages/PageRouting";
import { UserPage } from "./pages/UserPage";
import { ProjectsPage } from "./pages/ProjectsPage";
import { CreateProjectPage } from "./pages/CreateProjectPage";
import { Page } from "./pages/Page";
import { ProjectPage } from "./pages/ProjectPage";
import { LogInPage } from "./pages/LogInPage";
import { RegisterPage } from "./pages/RegisterPage";

interface AppState {
    url: URL,
    page?: any | null;
}

type PageParameters<T> = T extends Page<any, infer U> ? U : never;

export class App extends React.Component<{}, AppState> {
    constructor(props: {}) {
        super(props);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.state = {
            url: new URL(document.location.href)
        };
    }

    changePage(url: string)
    changePage<T>(url: string, state: T)
    changePage(url: string, state?: object) {
        history.pushState(state, "Test", url);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.setState({ url: new URL(document.location.href) });
    }

    change<T extends Page, P extends PageParameters<T>>(page: new(...args : any) => T, url: string, state: P) {
        history.pushState(state, "Test", url);

        if (document.location == null)
            throw new Error("The document does not provide a location");

        this.setState({ url: new URL(document.location.href) });
    }

    render() {
        return <div className="app">
            <NavigationBar />
            <div style={{ display: "flex", flexGrow: 1 }}>
                <PageRouting url={this.state.url} state={history.state}>
                    <Route route="/" page={HomePage}/>
                    <Route route="/Register" page={RegisterPage}/>
                    <Route route="/Login" page={LogInPage}/>
                    <Route route="/Dashboard" page={UserPage} />
                    <Route route="/Projects" page={ProjectsPage} />
                    <Route route="/CreateProject" page={CreateProjectPage} />
                    <Route route="/Project" page={ProjectPage} />
                </PageRouting>
            </div>
        </div>;
    }

    componentDidMount() {
        window.addEventListener("popstate", () => {
            this.setState({ url: new URL(document.location.href) });
        });
    }
}