import requests


def test_add_new_tag_task():
    url = 'https://localhost:5001/api/Task/AddTag'

    taskTag = {"TagId": 1, "TaskId": 1}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=taskTag, timeout=5, verify=False)
    assert resp.status_code == 200

    taskTag = {"TagId": 2, "TaskId": 2}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=taskTag, timeout=5, verify=False)
    assert resp.status_code == 200

    taskTag = {"TagId": 1, "TaskId": 2}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=taskTag, timeout=10, verify=False)
    assert resp.status_code == 200


def test_add_duplicated_tag_task():
    url = 'https://localhost:5001/api/Task/AddTag'
    taskTag = {"TagId": 1, "TaskId": 1}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=taskTag, timeout=100, verify=False)
    assert resp.status_code == 500


def test_add_not_existing_tag_to_task():
    url = 'https://localhost:5001/api/Task/AddTag'
    taskTag = {"TagId": 10, "TaskId": 1}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=taskTag, timeout=100, verify=False)
    assert resp.status_code == 500


def test_get_tag():
    url = 'https://localhost:5001/api/Tag/1'
    resp = requests.get(url, timeout=5, verify=False)
    assert resp.status_code == 200
    assert len(resp.json()["tasks"]) == 2
    assert resp.json()["tasks"][0]["taskId"] == 1
    assert resp.json()["tasks"][1]["taskId"] == 2


def test_get_task():
    url = 'https://localhost:5001/api/Task/2'
    resp = requests.get(url, timeout=5, verify=False)
    assert resp.status_code == 200
    assert len(resp.json()["tags"]) == 2
    assert resp.json()["tags"][0]["tagId"] == 1
    assert resp.json()["tags"][1]["tagId"] == 2


