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
  setAccessToken: (
    tk: string | null,
    refreshToken?: string) => void,
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
  setAccessToken: function (tk: string | null, refreshToken?: string): void {
    throw new Error('Function not implemented.');
  },
  notifications: [],
  setColorMode: function (tk: string | null): void {
    throw new Error('Function not implemented.');
  },
  colorMode: ''
}
export const GlobalState = createContext<GlobalStateProp>(initState);

export const GlobalStateProvider = ({ children }: { children: ReactNode }) => {
  const [getColorMode, setColorModeStorage] = useColorMode();
  const [getToken, setToken, removeToken] = useLocalStorage("ACCESS_TOKEN", "");
  const [_g3, setRefreshToken, removeRefreshToken] = useLocalStorage("REFRESH_TOKEN", "");

  const [user, setUser] = useState<User>(initState.user);
  const [accessToken, _setAccessToken] = useState<string>(getToken)
  const [notifications, setNotifications] = useState<Array<Notification>>(initState.notifications)
  const [colorMode, _setColorMode] = useState(getColorMode)

  const setAccessToken: (tk: string | null, refreshToken?: string) => void = (tk: string | null, refreshToken?: string) => {
    if (tk != null && refreshToken) {
      _setAccessToken(tk)
      setToken(tk);
      setRefreshToken(refreshToken);
    } else {      
      removeToken()
      removeRefreshToken()
      _setAccessToken("")
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
    const fetchUserInfo = async () => {

    }

    if(accessToken!=="") {
      fetchUserInfo();
    }

    return () => {}
  }, [accessToken])
  
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