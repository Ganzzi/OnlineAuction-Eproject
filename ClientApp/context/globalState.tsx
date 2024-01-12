import useLocalStorage from '@/hooks/useLocalStorage';
import { Notification } from '@/types/models/notification';
import { User } from '@/types/models/user';
import { ReactNode, createContext, useContext, useEffect, useState } from 'react';

type GlobalStateProp = {
  user: User,
  accessToken: string,
  setUser: (user: User) => void,
  setAccessToken: (tk: string | null) => void,
  notifications: Notification[]
}

const initState: GlobalStateProp = {
  user: {
    userId: 0,
    username: '',
    email: undefined,
    password: undefined,
    role: undefined,
    refreshToken: undefined,
    locked: undefined,
    resetPasswordToken: undefined,
    bids: undefined
  },
  accessToken: '',
  setUser: function (user: User): void {
    throw new Error('Function not implemented.');
  },
  setAccessToken: function (tk: string | null): void {
    throw new Error('Function not implemented.');
  },
  notifications: []
}
export const GlobalState = createContext<GlobalStateProp>(initState);

export const GlobalStateProvider = ({ children }: { children: ReactNode }) => {
  const [_g, _s, removeToken] = useLocalStorage("ACCESS_TOKEN", "");

  const [user, setUser] = useState<User>(initState.user);
  const [accessToken, _setAccessToken] = useState<string>(initState.accessToken)
  const [notifications, setNotifications] = useState<Array<Notification>>(initState.notifications)

  const setAccessToken: (tk: string | null) => void = (tk: string | null) => {
    if (tk != null) {
      _setAccessToken(tk)
    } else {
      removeToken()
    }
  }

  // use effect to get notifications
  useEffect(() => {
    
    return () => {}
  }, [user, accessToken])
  
  return (
    <GlobalState.Provider value={{
      user,
      setUser,
      accessToken,
      setAccessToken,
      notifications
    }}>
      {children}
    </GlobalState.Provider>
  );
};

export const useGlobalState: () => GlobalStateProp = () => {
  const { 
    user,
    setUser,
    accessToken,
    setAccessToken,
    notifications 
  } = useContext(GlobalState);

  return {
    user,
    setUser,
    accessToken,
    setAccessToken,
    notifications
  };
};