import { useEffect, useState, Dispatch, SetStateAction } from 'react';

type SetValue<T> = T | ((val: T) => T);

type UseLocalStorageReturnType<T> = [T, Dispatch<SetStateAction<T>>, () => void];

function useLocalStorage<T>(
  key: string,
  initialValue: T
): UseLocalStorageReturnType<T> {
  // State to store our value
  const [storedValue, setStoredValue] = useState<T>(() => {
    try {
      if (typeof window !== 'undefined') {
        const item = window.localStorage.getItem(key);
        return item ? JSON.parse(item) : initialValue;
      } else {
        return initialValue;
      }
    } catch (error) {
      console.error(error);
      return initialValue;
    }
  });

  const removeFromLocalStorage = () => {
    try {
      if (typeof window !== 'undefined') {
        window.localStorage.removeItem(key);
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    try {
      if (typeof window !== 'undefined') {
        const valueToStore =
          typeof storedValue === 'function'
            ? (storedValue as (val: T) => T)(storedValue)
            : storedValue;
        window.localStorage.setItem(key, JSON.stringify(valueToStore));
      }
    } catch (error) {
      console.error(error);
    }
  }, [key, storedValue]);

  return [storedValue, setStoredValue, removeFromLocalStorage];
}

export default useLocalStorage;
