import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className="dark:bg-boxdark-2 dark:text-bodydark">
      <div className="p-4 md:p-6 2xl:p-10">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <h2 className="text-xl font-bold mb-2">Contact Us</h2>
            <p>Email: info@example.com</p>
            <p>Phone: +1 123-456-7890</p>
          </div>
          <div>
            <h2 className="text-xl font-bold mb-2">Follow Us</h2>
            <p>Twitter: @example_twitter</p>
            <p>Instagram: @example_instagram</p>
          </div>
        </div>

        <hr className="my-4 border-t border-gray-300" />

        <p className="text-sm">
          &copy; 2024 Your Website Name. All rights reserved.
        </p>
      </div>
    </footer>
  );
};

export default Footer;
