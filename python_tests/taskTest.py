import requests


url = 'https://localhost:5001/api/Task'
headers = {'Content-Type': 'application/json'}


def test_add_new_task():
    url = 'https://localhost:5001/api/Task'
    task = {"Name": "Task1", "Description": "Opis taska 1"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=task, timeout=5, verify=False)
    assert resp.status_code == 200

    url = 'https://localhost:5001/api/Task'
    task = {"Name": "Task2", "Description": "Opis taska 2"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=task, timeout=5, verify=False)
    assert resp.status_code == 200


def test_get_task():
    url = 'https://localhost:5001/api/Task/1'
    resp = requests.get(url, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.json()['name'] == 'Task1'
    assert resp.json()["description"] == "Opis taska 1"

