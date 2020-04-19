import * as React from "react";
import { BindableComponent } from "../components/BindableComponent";

export interface PageProps<PR> {
    params? : Readonly<PR>
}

export class Page<S = {}, PR = {}> extends BindableComponent<PageProps<PR>, S> {
    params: Partial<Readonly<PR>>;

    constructor(props: PageProps<PR>) {
        super(props);

        this.params = props.params || {} as Readonly<PR>;
    }

    render(): React.ReactNode {
        return null;
    }
}