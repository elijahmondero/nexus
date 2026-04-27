import { useState, useEffect } from 'react';
import client from '../api/client';

interface {{Name}} {
  id: string;
  name: string;
}

const use{{Name}} = () => {
  const [items, setItems] = useState<{{Name}}[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    client.get('/api/{{Name}}')
      .then(res => setItems(res.data))
      .finally(() => setLoading(false));
  }, []);

  return { items, loading };
};

export default use{{Name}};
