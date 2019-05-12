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
}