'use client'

import useColorMode from '@/hooks/useColorMode';
import useLocalStorage from '@/hooks/useLocalStorage';
import { Notification } from '@/types/models/notification';
import { User } from '@/types/models/user';
import { ReactNode, createContext, useContext, useEffect, useState } from 'react';
import { auctionHistory1, auctionHistory2, bid1, bid2, item1, item2, user1 } from '@/data/item';
import axiosService from '@/services/axiosService';
import signalRService from '@/services/signalRService';

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
      setToken(tk);
      setRefreshToken(refreshToken);
      _setAccessToken(tk)
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
      const res = await axiosService.get("/api/user/Profile");
      const user: User = res.data.user;

      await signalRService.startConnection(user.userId);

      setUser(user);
    }

    if (accessToken !== "") {
      fetchUserInfo();
    }

    return () => {
      if (accessToken !== "" && user.userId != 0) {
        signalRService.closeConnection(user.userId);
      }
    }
  }, [accessToken])

  useEffect(() => {
    const handleSomeoneJoinItemRoom = (itemId: number, userId: number, bidAmount: number) => {
      console.log(`${userId} has joined item ${itemId}`);
      
    };

    const handleAuctionEnded = (itemId: number, sellerId: number) => {
      console.log(itemId + " has ended");
    };

    // Subscribe to the event when the component mounts
    signalRService.onSomeoneJoinItemRoom(handleSomeoneJoinItemRoom);
    signalRService.onAuctionEnded(handleAuctionEnded);

    // // Unsubscribe when the component unmounts
    return () => {
      signalRService.offSomeoneJoinItemRoom(handleSomeoneJoinItemRoom);
      signalRService.offAuctionEnded(handleAuctionEnded);
    };
  });

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
    notifications,
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