import * as React from "react";

export interface PageProps<PR> {
    params? : Readonly<PR>
}

export class Page<P = {}, S = {}, PR = {}> extends React.Component<P & PageProps<PR>, S> {
    params: Partial<Readonly<PR>>;

    constructor(props: P & PageProps<PR>) {
        super(props);

        this.params = props.params || {} as Readonly<PR>;
    }
}