import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 100,
  duration: '30s',
};

export default function () {
  const res = http.get('https://localhost:55539/users', { 
    insecureSkipTLSVerify: true, // Ignore self-signed certificate
  });

  check(res, {
    'status is 200': (r) => r.status === 200,
  });
}