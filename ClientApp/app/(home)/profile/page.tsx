'use client'

import { useGlobalState } from "@/context/globalState";
import Image from "next/image";
import { useEffect, useState } from "react";
import { User } from "@/types/models/user";
import axiosService from "@/services/axiosService";
import { useRouter } from "next/navigation";
import { Bid } from "@/types/models/bid";
import { AuctionHistory } from "@/types/models/auctionHistory";
import { Item } from "@/types/models/item";


const ProfilePage = () => {
    const { user } = useGlobalState();
    const [userData, setUserData] = useState<User>(user);
    const router = useRouter();

    useEffect(() => {
        const fetchUserProfile = async () => {
            const res = await axiosService.get('/api/user/Profiledetail');
            const data: User = res.data.user

            setUserData(data);
        }

        fetchUserProfile()
    }, [])


    return (
        <>
            <div className="overflow-hidden rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                <div className="relative z-20 h-35 md:h-65">
                    <Image
                        src={"/images/cover/cover-01.png"}
                        alt="profile cover"
                        className="h-full w-full rounded-tl-sm rounded-tr-sm object-cover object-center"
                        width={970}
                        height={260}
                    />
                </div>
                <div className="px-4 pb-6 text-center lg:pb-8 xl:pb-11.5">
                    <div className="relative z-30 mx-auto -mt-22 h-30 w-full max-w-30 rounded-full bg-white/20 p-1 backdrop-blur sm:h-44 sm:max-w-44 sm:p-3">
                        <div className="relative drop-shadow-2">
                            <img
                            className="rounded-full"
                                src={userData?.avatar}
                                width={160}
                                height={160}
                                alt="profile"
                            />
                            <label
                                htmlFor="profile"
                                onClick={() => router.push('/profile/update')}
                                className="absolute bottom-0 right-0 flex h-8.5 w-8.5 cursor-pointer items-center justify-center rounded-full bg-primary text-white hover:bg-opacity-90 sm:bottom-2 sm:right-2"
                            >
                                <svg
                                    className="fill-current"
                                    width="14"
                                    height="14"
                                    viewBox="0 0 14 14"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        fillRule="evenodd"
                                        clipRule="evenodd"
                                        d="M2.965 10.633C2.73 10.868 2.357 10.868 2.122 10.633L0.235 8.747C0 8.513 0 8.14 0.235 7.905L8.535 0.606C8.77 0.371 9.143 0.371 9.378 0.606L11.265 2.492C11.5 2.727 11.5 3.1 11.265 3.335L3.965 10.633C3.73 10.868 3.357 10.868 3.122 10.633V10.633ZM11.557 1.115L12.885 2.443L13.557 1.771C14.025 1.303 14.975 1.303 15.443 1.771L16.828 3.156C17.297 3.625 17.297 4.575 16.828 5.043L5.5 16.371C5.032 16.839 4.082 16.839 3.614 16.371L1.828 14.586C1.36 14.118 1.36 13.168 1.828 12.7L13.157 1.371C13.625 0.902 14.575 0.902 15.043 1.371L16.828 3.157C17.297 3.625 17.297 4.575 16.828 5.043L11.557 1.115Z"
                                    />
                                </svg>
                            </label>
                        </div>
                    </div>
                    <div className="mt-4">
                        <h3 className="mb-1.5 text-2xl font-semibold text-black dark:text-white">
                            {userData?.name}
                        </h3>
                        <p className="font-medium">{userData?.email}</p>
                        
                    </div>
                </div>

                <BidsList bids={userData?.bids?.reverse()} />

                <div className="flex space-x-8 px-20 mb-10 text-gray-2">
                    <div className="w-1/2 bg-body p-5 rounded-md">
                        <SoldItemsList soldItems={userData?.soldItems?.reverse()} />
                    </div>
                    <div className="w-1/2 bg-body p-5 rounded-md">
                        <BoughtItemsList auctionHistories={userData?.auctionHistories?.reverse()} />
                    </div>
                </div>
            </div>
        </>
    );
};

type SoldItemsListProps = {
    soldItems?: Item[];
};

const SoldItemsList: React.FC<SoldItemsListProps> = ({ soldItems }) => {
    const router = useRouter();

    return (
        <div>
            <h2 className="text-xl font-semibold mb-4 text-center">Sold Items</h2>
            <ul className="list-disc px-4">
                {!soldItems || soldItems.length == 0 && (
                    <p>No Item Sold</p>
                )}
                {soldItems?.map((item) => (
                    <li
                        key={item.itemId}
                        onClick={() => router.push(`/profile/auction/${item?.auctionHistory?.auctionHistoryId}`)}
                        className="mb-2 hover:bg-meta-3 px-2 py-1 flex flex-row items-center">
                        <img src={item?.image} width={30} height={30} className="mr-2" alt="" /> {item.title} - ${item.startingPrice.toFixed(2)}
                    </li>
                ))}
            </ul>
        </div>
    );
};

type BoughtItemsListProps = {
    auctionHistories?: AuctionHistory[];
};

const BoughtItemsList: React.FC<BoughtItemsListProps> = ({ auctionHistories }) => {
    const router = useRouter();

    return (
        <div className="">
            <h2 className="text-xl font-semibold mb-4 text-center">Successfully bought items</h2>
            <ul className="list-disc px-4">
                {!auctionHistories || auctionHistories.length == 0 && (
                    <p>No item bought</p>
                )}
                {auctionHistories?.map((auc) => (
                    <li
                        key={auc.auctionHistoryId}
                        onClick={() => router.push(`/profile/auction/${auc.auctionHistoryId}`)}
                        className="mb-2 hover:bg-meta-3 px-2 py-1 flex flex-row items-center">
                        <img src={auc?.item?.image} width={30} height={30} className="mr-2" alt="" />{auc?.item?.title} - ${auc?.winningBid.toFixed(2)} - {new Date(auc.endDate).toLocaleString()}
                    </li>
                ))}
            </ul>
        </div>
    );
};

type BidsListProps = {
    bids?: Bid[];
};

const BidsList: React.FC<BidsListProps> = ({ bids }) => {
    const router = useRouter();

    return (
        <div className="space-x-8 mx-20 mb-10 p-3 text-gray-2 bg-body rounded-md">
            <h2 className="text-xl font-semibold mb-4 text-center">Placed Bids</h2>
            <ul className="list-disc px-4">
                {!bids || bids.length == 0 && (
                    <p>No bid placed</p>
                )}
                {bids?.map((bid) => (
                    <li key={bid.bidId} className="mb-2 hover:bg-meta-3 px-2 py-1 flex flex-row justify-between items-start"
                        onClick={() => router.push(`/items/${bid?.item?.itemId}`)}

                    >
                        <p>
                            ${bid.bidAmount?.toFixed(2)} - {new Date(bid.bidDate).toLocaleString()}
                        </p>
                        <div className="flex flex-row">
                            <p>{bid?.item?.title}</p>
                            <img src={bid?.item?.image} width={30} height={30} className="ml-2" alt="" />
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ProfilePage;
