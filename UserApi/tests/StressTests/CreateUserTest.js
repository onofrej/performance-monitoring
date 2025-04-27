import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [
    { duration: '20s', target: 10 }, // Simulate 20 users over 30 seconds
  ],
};

export default function () {
  const url = 'https://kjfkq6uahrpl3uoc3ehtl4323e0ixqkc.lambda-url.us-east-1.on.aws/users';
  const payload = JSON.stringify({
    baptismDate: "2024-07-19T19:23:02.918Z",
    birthDate: "2024-01-19T19:23:02.918Z",
    gender: 1,
    congregationId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    privilege: 1,
    email: "onofre.junior@gmail.com",
    name: "Onofre",
    phoneNumber: "11997014906",
    grade: 1
  });

  const params = {
    headers: {
      'accept': '*/*',
      'Content-Type': 'application/json',
    },
  };

  const res = http.post(url, payload, params);

  // Check the response status and print detailed error messages if the request fails
  check(res, {
    'status is 200': (r) => r.status === 200,
    'status is 201': (r) => r.status === 201,
  }) || console.error(`Error: Status code was ${res.status}\nResponse body: ${res.body}`);
  
  sleep(1); // Simulate user think time
}