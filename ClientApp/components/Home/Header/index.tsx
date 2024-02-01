import Link from "next/link";
import DarkModeSwitcher from "../../Dashboard/Header/DarkModeSwitcher";
import DropdownMessage from "../../Dashboard/Header/DropdownMessage";
import DropdownNotification from "../../Dashboard/Header/DropdownNotification";
import DropdownUser from "../../Dashboard/Header/DropdownUser";
import Image from "next/image";
import { usePathname, useRouter } from "next/navigation";
import { FormEvent, useState } from "react";
import { useGlobalState } from "@/context/globalState";

const Header = () => {
  const { user } = useGlobalState();
  const pathname = usePathname();

  return (
    <header className="sticky top-0 z-999 flex w-full bg-body drop-shadow-1 dark:bg-boxdark dark:drop-shadow-none">
      <div className="flex flex-grow items-center justify-between px-4 py-4 shadow-2 md:px-6 2xl:px-11">
        <div className='space-x-10 flex flex-row items-center justify-start text-white'>
          <Link href={'/'} className={`${!pathname.startsWith("/items") && !pathname.startsWith("/profile") ? "text-meta-7" : ""}`}>
            <Image
              className=""
              src={"/images/logo/newlogo.png"}
              alt="Logo"
              width={176}
              height={32}
            />
          </Link>
          <Link href={'/items'} className={`${pathname.startsWith("/items") ? "text-meta-7" : ""}`}>Market</Link>
          <Search />
        </div>

        <div className="flex items-center gap-3 2xsm:gap-7">
          <ul className="flex items-center gap-2 2xsm:gap-4">
            {/* <!-- Dark Mode Toggler --> */}
            <DarkModeSwitcher />
            {/* <!-- Dark Mode Toggler --> */}

            {/* <!-- Notification Menu Area --> */}
            {user.userId != 0 && (<DropdownNotification />)}
            {/* <!-- Notification Menu Area --> */}
          </ul>

          {/* <!-- User Area --> */}
          {user.userId != 0 ? (
            <DropdownUser />
          ) : (
            <div className="flex flex-row items-center space-x-3">
              <Link href={'/auth/signin'} className={`hover:text-meta-5`}>Signin</Link >
              <p>/</p>
              <Link href={'/auth/signup'} className={`hover:text-meta-5`}>SignUp</Link >
            </div>
          )}
          {/* <!-- User Area --> */}
        </div>
      </div>
    </header>
  );
};

const Search = () => {
  const router = useRouter();
  const [searchQuery, setSearchQuery] = useState<string>('');

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (searchQuery.trim()) {
      // Call the onSubmit callback with the search query
      router.push(`/items?search=${encodeURIComponent(searchQuery)}`);

      // Reset the form
      setSearchQuery('');

      // Alternatively, you can navigate to the /items page directly here if needed
      // router.push(`/items?search=${encodeURIComponent(searchQuery)}`);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        name="search"
        placeholder="Search..."
        className="p-2 rounded-lg dark:bg-gray dark:text-boxdark bg-bodydark text-gray-3"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
    </form>
  );
};

export default Header;
