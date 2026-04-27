import { useState, useEffect } from 'react';
import client from '../api/client';

export interface {{Name}} {
  id: string;
  name: string;
}

const use{{Name}} = () => {
  const [items, setItems] = useState<{{Name}}[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchItems = () => {
    setLoading(true);
    client.get('/api/{{Name}}')
      .then(res => setItems(res.data))
      .finally(() => setLoading(false));
  };

  const createItem = async (name: string) => {
    await client.post('/api/{{Name}}', { name });
    fetchItems();
  };

  useEffect(() => {
    fetchItems();
  }, []);

  return { items, loading, createItem };
};

export default use{{Name}};
