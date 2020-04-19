export class User {
    static async logIn(username: string, password: string): Promise<boolean> {
        const response = await fetch("api/User/Login", {
            method: "POST",
            body: JSON.stringify({ username, password }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        return result;
    }

    static async register(type: "employee" | "company", name: string, lastName: string, location: string, username: string, password: string, email: string): Promise<boolean> {
        const response = await fetch("api/User/Register", {
            method: "POST",
            body: JSON.stringify({ type, name, lastName, location, username, password, email }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        return result;
    }
}