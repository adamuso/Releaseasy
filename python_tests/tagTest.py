import requests


def test_add_new_tag():
    url = 'https://localhost:5001/api/Tag'
    #headers = {'Content-Type': 'application/json'}
    tag = {"Name": "Tag 1"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=tag, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.text == '1'
    tag = {"Name": "Tag 2"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=tag, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.text == '1'
    tag = {"Name": "Tag 3"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=tag, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.text == '1'


def test_add_duplicated_tag():
    url = 'https://localhost:5001/api/Tag'
    tag = {"Name": "Tag 2"}
    resp = requests.post(url, headers={'Content-Type': 'application/json'}, json=tag, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.text == '-1'


def test_get_tag():
    url = 'https://localhost:5001/api/Tag/1'
    resp = requests.get(url, timeout=5, verify=False)
    assert resp.status_code == 200
    assert resp.json()['name'] == 'Tag 1'
