'use client'

import useColorMode from '@/hooks/useColorMode';
import useLocalStorage from '@/hooks/useLocalStorage';
import { Notification } from '@/types/models/notification';
import { User } from '@/types/models/user';
import { ReactNode, createContext, useContext, useEffect, useState } from 'react';
import { auctionHistory1, auctionHistory2, bid1, bid2, item1, item2, user1 } from '@/data/item';

type GlobalStateProp = {
  user: User,
  accessToken: string,
  setUser: (user: User) => void,
  setAccessToken: (tk: string | null) => void,
  notifications: Notification[],
  colorMode: string,
  setColorMode: (color: string) => void
}

const initState: GlobalStateProp = {
  user: {
    ...user1,
    soldItems: [
      item1,
      item2
    ], bids: [
      bid1, bid2
    ], auctionHistories: [
      auctionHistory1, auctionHistory2
    ]
  },
  accessToken: '',
  setUser: function (user: User): void {
    throw new Error('Function not implemented.');
  },
  setAccessToken: function (tk: string | null): void {
    throw new Error('Function not implemented.');
  },
  notifications: [],
  colorMode: localStorage.getItem("color-theme")?.toString() ?? "light",
  setColorMode: function (tk: string | null): void {
    throw new Error('Function not implemented.');
  },
}
export const GlobalState = createContext<GlobalStateProp>(initState);

export const GlobalStateProvider = ({ children }: { children: ReactNode }) => {
  const [_g1, setColorModeStorage] = useColorMode();
  const [_g2, _s2, removeToken] = useLocalStorage("ACCESS_TOKEN", "");

  const [user, setUser] = useState<User>(initState.user);
  const [accessToken, _setAccessToken] = useState<string>(initState.accessToken)
  const [notifications, setNotifications] = useState<Array<Notification>>(initState.notifications)
  const [colorMode, _setColorMode] = useState(initState.colorMode)

  const setAccessToken: (tk: string | null) => void = (tk: string | null) => {
    if (tk != null) {
      _setAccessToken(tk)
    } else {
      removeToken()
    }
  }

  const setColorMode = (colorMode: string) => {
    if (typeof setColorModeStorage === 'function') {
      setColorModeStorage(colorMode);
    }
    _setColorMode(colorMode);
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
      notifications,
      colorMode,
      setColorMode
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
    notifications ,
    colorMode, setColorMode
  } = useContext(GlobalState);

  return {
    user,
    setUser,
    accessToken,
    setAccessToken,
    notifications,
     colorMode, setColorMode
  };
};