import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 10,
  duration: '2s',
};

export default function () {
  const res = http.get('https://localhost:55539/users', { 
    insecureSkipTLSVerify: true,
  });

  check(res, {
    'status is 200': (r) => r.status === 200,
  });
}